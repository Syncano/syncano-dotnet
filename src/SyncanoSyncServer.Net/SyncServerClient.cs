using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveSockets;
using Syncano.Net;
using SyncanoSyncServer.Net.Notifications;
using SyncanoSyncServer.Net.RealTimeSyncApi;

namespace SyncanoSyncServer.Net
{
    /// <summary>
    /// Class may be used to connect to Syncano Sync Server over Tcp.
    /// </summary>
    public class SyncServerClient : IDisposable, ISyncanoClient
    {
        private ReactiveClient _client;
        private Subject<string> _messagesObservable = new Subject<string>();
        private string _sessionId = null;
        private string _authKey = null;

        /// <summary>
        /// Observable where you can subscribe on incoming messages as string objects.
        /// </summary>
        public IObservable<string> MessagesObservable
        {
            get { return _messagesObservable; }
        }

        private Subject<NewDataNotification> _newDataNotificationObservable = new Subject<NewDataNotification>();
        private Subject<DeleteDataNotification> _deleteDataNotificationObservable = new Subject<DeleteDataNotification>();
        private Subject<ChangeDataNotification> _changeDataNotificationObservable = new Subject<ChangeDataNotification>();
        private Subject<DataRelationNotification> _dataRelationNotificationObservable = new Subject<DataRelationNotification>();
        private Subject<GenericNotification> _genericNotificationObservable = new Subject<GenericNotification>();

        /// <summary>
        /// Observable providing notifications about new DataObjects in Syncano Instance.
        /// </summary>
        public IObservable<NewDataNotification> NewDataNotificationObservable
        {
            get { return _newDataNotificationObservable; }
        }

        /// <summary>
        /// Observable providing notifications about deleted DataObjects in Syncano Instance.
        /// </summary>
        public IObservable<DeleteDataNotification> DeleteDataNotificationObservable
        {
            get { return _deleteDataNotificationObservable; }
        }

        /// <summary>
        /// Observable providing notifications about modified DataObjects in Syncano Instance.
        /// </summary>
        public IObservable<ChangeDataNotification> ChangeDataNotificationObservable
        {
            get { return _changeDataNotificationObservable; }
        }

        /// <summary>
        /// Observable providing notifications about DataObjects relations in Syncano Instance.
        /// </summary>
        public IObservable<DataRelationNotification> DataRelationNotificationObservable
        {
            get { return _dataRelationNotificationObservable; }
        }

        /// <summary>
        /// Observable providing some generic notifications about Syncano Instance (ex. new server logged in or custom notification send by other user).
        /// </summary>
        public IObservable<GenericNotification> GenericNotificationObservable
        {
            get { return _genericNotificationObservable; }
        }

        /// <summary>
        /// Boolen value with information about connection status. True means that SyncServer object is connected with Syncano Sync Server.
        /// </summary>
        public bool IsConnected
        {
            get { return _client.IsConnected && _isAuthenticated; }
        }

        private ConcurrentStack<byte> _messageByteStack = new ConcurrentStack<byte>();
        private static long _currentMessageId;

        /// <summary>
        /// Creates SyncServerClient client object.
        /// </summary>
        public SyncServerClient()
        {
            _client = new ReactiveClient("api.syncano.com", 8200, stream =>
            {
                _ssl = new SslStream(stream);
                _ssl.AuthenticateAsClient("api.syncano.com");
                return _ssl;
            });

            _newBytesSubscription = _client.Receiver.SubscribeOn(TaskPoolScheduler.Default).Subscribe(AddNewByte);
            _messagesSubscription = _messagesObservable.SubscribeOn(TaskPoolScheduler.Default).Subscribe(OnNewMessage);

            _every10seconds = Observable.Interval(TimeSpan.FromSeconds(10)).SubscribeOn(TaskPoolScheduler.Default).Subscribe(_ => OnEvery10Seconds());
        }

        private readonly ConcurrentDictionary<string, ISyncanoConversation> _conversationsByMessageId = new ConcurrentDictionary<string, ISyncanoConversation>();

        private void OnEvery10Seconds()
        {
            var conversationsToRemove = _conversationsByMessageId.Values.Where(c => c.HasCompleted).Select(c => c.Request.MessageId.ToString()).ToList();

            ISyncanoConversation conversation;
            conversationsToRemove.ForEach(id => _conversationsByMessageId.TryRemove(id, out conversation));

            foreach (var eachConversation in _conversationsByMessageId.Values)
            {
                eachConversation.VerifyTimeout();

                if (eachConversation.WasTimeouted)
                    RelaseOneSemaphoreEntry();
            }
        }


        private readonly Regex _callResponseRegex = new Regex("\"type\":\"callresponse\",\"message_id\":(?<MessageId>\\d+)");
        private readonly Regex _newNotificationRegex = new Regex("\"type\":\"new\"");
        private readonly Regex _deleteNotificationRegex = new Regex("\"type\":\"delete\"");
        private readonly Regex _changeNotificationRegex = new Regex("\"type\":\"change\"");

        private readonly Regex _dataNotificationRegex = new Regex("\"object\":\"data\"");
        private readonly Regex _dataRelationNotificationRegex = new Regex("\"object\":\"datarelation\"");
        private readonly Regex _genericNotificationRegex = new Regex("\"object\":\"me\"");

        private void OnNewMessage(string message)
        {
            var match = _callResponseRegex.Match(message);
            if (match.Success)
            {
                var messageId = match.Groups["MessageId"].Value;

                ISyncanoConversation conversation;
                if (_conversationsByMessageId.TryGetValue(messageId, out conversation))
                {
                    try
                    {
                        if (!conversation.HasCompleted)
                            conversation.SetResponse(message);
                    }
                    finally
                    {
                        RelaseOneSemaphoreEntry();
                    }
                }
            }
            else if (_dataNotificationRegex.IsMatch(message))
            {
                if (_newNotificationRegex.IsMatch(message))
                    _newDataNotificationObservable.OnNext(JsonConvert.DeserializeObject<NewDataNotification>(message));

                if (_deleteNotificationRegex.IsMatch(message))
                    _deleteDataNotificationObservable.OnNext(
                        JsonConvert.DeserializeObject<DeleteDataNotification>(message));

                if (_changeNotificationRegex.IsMatch(message))
                    _changeDataNotificationObservable.OnNext(
                        JsonConvert.DeserializeObject<ChangeDataNotification>(message));
            }
            else if (_dataRelationNotificationRegex.IsMatch(message))
            {
                _dataRelationNotificationObservable.OnNext(
                    JsonConvert.DeserializeObject<DataRelationNotification>(message));
            }
            else if (_genericNotificationRegex.IsMatch(message))
            {
                _genericNotificationObservable.OnNext(JsonConvert.DeserializeObject<GenericNotification>(message));
            }
        }


        private bool _isAuthenticated;
        private IDisposable _newBytesSubscription;


        private void AddNewByte(byte newByte)
        {
            if (IsMessageSeparator(newByte))
            {
                byte[] messageBytes = new byte[_messageByteStack.Count];
                _messageByteStack.TryPopRange(messageBytes);
                string message = Encoding.UTF8.GetString(messageBytes.Reverse().ToArray());
                _messagesObservable.OnNext(message);
            }
            else
            {
                _messageByteStack.Push(newByte);
            }
        }

        private static bool IsMessageSeparator(byte newByte)
        {
            return newByte == 10;
        }

        /// <summary>
        /// Connects SyncServerClient to Syncano SyncServer.
        /// </summary>
        /// <returns>Task performing connection operation.</returns>
        public Task Connect()
        {
            return _client.ConnectAsync();
        }

        /// <summary>
        /// Disconnects SyncServerClient form Syncano Sync Server.
        /// </summary>
        public void Disconnect()
        {
            _client.Disconnect();
        }

        /// <summary>
        /// Logs in to specified Syncano Instance over specified ApiKey.
        /// </summary>
        /// <param name="apiKey">Api Key of Syncano Instance (user or backend).</param>
        /// <param name="instanceName">Name of Syncano Instance.</param>
        /// <returns>Task performing login operation and returning LoginResult object.</returns>
        public Task<LoginResult> Login(string apiKey, string instanceName)
        {
            var request = new LoginRequest {InstanceName = instanceName, ApiKey = apiKey, AuthKey = _authKey, SessionId = _sessionId};

            var t = _messagesObservable.FirstAsync().Select(ToLoginResult)
                .FirstAsync().Timeout(TimeSpan.FromSeconds(10))
                .Do(r => { _isAuthenticated = r.WasSuccessful; })
                .ToTask();

            _client.SendAsync(CreateRequest(request));

            return t;
        }

        private static LoginResult ToLoginResult(string s)
        {
            if (s.Contains("\"result\":\"OK\""))
            {
                var response = JsonConvert.DeserializeObject<LoginResponse>(s);
                return new LoginResult(true);
            }
            else
            {
                return new LoginResult(false);
            }
        }

        private long GetMessageId()
        {
            return Interlocked.Add(ref _currentMessageId, 1);
        }

        
        private Task<T> SendCommandAsync<T>(ApiCommandRequest request, string contentToken)
        {
            return SendCommandAsync<T>(request, jo => jo.SelectToken("data").SelectToken(contentToken).ToObject<T>());
        }

        private Task<T> SendCommandAsync<T>(ApiCommandRequest request, Func<JToken, T> getResult)
        {
            var syncanoConversation = new SyncanoConversation<T>(request, getResult);
            _conversationsByMessageId.TryAdd(syncanoConversation.Id, syncanoConversation);

            SendRequestAsync(syncanoConversation);

            return syncanoConversation.ResponseObservable.ToTask();
        }


        private ApiCommandRequest CreateCommandRequest(string methodName, object parameters)
        {
            var request = new ApiCommandRequest(methodName, GetMessageId());


            if (parameters != null)
            {
                foreach (var eachProperty in parameters.GetType().GetProperties())
                {
                    if (eachProperty.GetValue(parameters) != null)
                    {
                        if (eachProperty.GetValue(parameters).GetType().IsConstructedGenericType &&
                            eachProperty.GetValue(parameters).GetType().GetGenericTypeDefinition() == typeof (Dictionary<,>))
                        {
                            if (eachProperty.GetValue(parameters) is Dictionary<string, string>)
                            {
                                var dictionary = (Dictionary<string, string>) eachProperty.GetValue(parameters);
                                foreach (var item in dictionary)
                                    request.Params.Add(item.Key, item.Value);
                            }
                            else
                            {
                                var dictionary = (Dictionary<string, object>) eachProperty.GetValue(parameters);
                                foreach (var item in dictionary)
                                    request.Params.Add(item.Key, item.Value);
                            }
                        }
                        else
                            request.Params.Add(eachProperty.Name, eachProperty.GetValue(parameters));
                    }
                }
            }
            return request;
        }

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(8, 8);
        private IDisposable _messagesSubscription;
        private bool _isDisposing;
        private IDisposable _every10seconds;
        private SslStream _ssl;

        private void RelaseOneSemaphoreEntry()
        {
            if (!_isDisposing)
                _semaphore.Release();
        }

        private async Task SendRequestAsync(ISyncanoConversation conversation)
        {
            if (_isAuthenticated == false) throw new SyncanoException("Cannot send any Requests when not authenticated");

            var result = await _semaphore.WaitAsync(TimeSpan.FromSeconds(20));
            if (!result)
            {
                conversation.SetError(new SyncanoException("Timeout - wait time in queue to send request has been exceeded"));
                return;
            }

            try
            {
                await _client.SendAsync(CreateRequest(conversation.Request));
                conversation.SetSent();
            }
            catch (Exception ex)
            {
                RelaseOneSemaphoreEntry();
                conversation.SetError(ex);
                throw;
            }
        }
        

        private byte[] CreateRequest(ISyncanoRequest request)
        {
            string cmd = JsonConvert.SerializeObject(request) + "\n";

            var messageBytes = Encoding.UTF8.GetBytes(cmd);
            if (messageBytes.Length > 128*1024)
                throw new SyncanoException("Request size exceeded, maximum size 128Kb");

            return messageBytes;
        }

        /// <summary>
        /// Disposes SyncServerCLient object.
        /// </summary>
        public void Dispose()
        {
            _isDisposing = true;
            if (_newBytesSubscription != null)
                _newBytesSubscription.Dispose();

            if (_messagesSubscription != null)
                _messagesSubscription.Dispose();

            if (_every10seconds != null)
                _every10seconds.Dispose();

            foreach (var syncanoConversation in _conversationsByMessageId)
            {
                if (!syncanoConversation.Value.HasCompleted)
                    syncanoConversation.Value.SetError(new SyncanoException("SyncServerClient was disposed"));
            }

            _semaphore.Dispose();

            if (_ssl != null)
            {
                _ssl.Dispose();
            }

            if (_client != null && _client.IsConnected)
            {
                _client.Dispose();
            }
        }

        /// <summary>
        /// Method of sending messages to Syncano.
        /// </summary>
        /// <param name="methodName">Name of Syncano Rest Api method.</param>
        /// <param name="parameters">Object containg proper parameters.</param>
        /// <returns>Boolean value indicating operation success.</returns>
        public Task<bool> GetAsync(string methodName, object parameters)
        {
            var request = CreateCommandRequest(methodName, parameters);
            return SendCommandAsync<bool>(request, jo => jo.SelectToken("result").Value<string>() == "OK");
        }

        /// <summary>
        /// Method of sending messages to Syncano.
        /// </summary>
        /// <typeparam name="T">Type to retrieve from Syncano.</typeparam>
        /// <param name="methodName">Name of Syncano Rest Api method.</param>
        /// <param name="contentToken">Token of response message marking object to retrieve.</param>
        /// <returns>Retrived object.</returns>
        public Task<T> GetAsync<T>(string methodName, string contentToken)
        {
            return GetAsync<T>(methodName, null, contentToken);
        }

        /// <summary>
        /// Method of sending messages to Syncano.
        /// </summary>
        /// <typeparam name="T">Type to retrieve from Syncano.</typeparam>
        /// <param name="methodName">Name of Syncano Rest Api method.</param>
        /// <param name="parameters">Object containg proper parameters.</param>
        /// <param name="contentToken">Token of response message marking object to retrieve.</param>
        /// <returns>Retrived object.</returns>
        public Task<T> GetAsync<T>(string methodName, object parameters, string contentToken)
        {
            var request = CreateCommandRequest(methodName, parameters);
            return SendCommandAsync<T>(request, contentToken);
        }

        /// <summary>
        /// Method of posting messages to Syncano.
        /// </summary>
        /// <typeparam name="T">Type to retrieve from Syncano.</typeparam>
        /// <param name="methodName">Name of Syncano Rest Api method.</param>
        /// <param name="parameters">Object containg proper parameters.</param>
        /// <param name="contentToken">Token of response message marking object to retrieve.</param>
        /// <returns>Retrived object.</returns>
        public Task<T> PostAsync<T>(string methodName, object parameters, string contentToken)
        {
            return GetAsync<T>(methodName, parameters, contentToken);
        }


        public void SetUserContext(string authKey)
        {
            _authKey = authKey;
        }

        public void SetSessionContext(string sessionId)
        {
            _sessionId = sessionId;
        }
    }
}