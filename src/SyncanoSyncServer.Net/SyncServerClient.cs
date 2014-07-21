using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Security;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveSockets;
using Syncano.Net;

namespace SyncanoSyncServer.Net
{
    public class SyncServerClient : IDisposable, ISyncanoClient
    {
        private ReactiveClient _client;
        private Subject<string> _messagesObservable = new Subject<string>();

        public IObservable<string> MessagesObservable
        {
            get { return _messagesObservable; }
        }

        public bool IsConnected
        {
            get { return _client.IsConnected && _isAuthenticated; }
        }

        private ConcurrentStack<byte> _messageByteStack = new ConcurrentStack<byte>();
        private static long _currentMessageId;

        public SyncServerClient()
        {
            _client = new ReactiveClient("api.syncano.com", 8200, stream =>
            {
                var ssl = new SslStream(stream);
                ssl.AuthenticateAsClient("api.syncano.com");
                return ssl;
            });

            _newBytesSubscription = _client.Receiver.SubscribeOn(TaskPoolScheduler.Default).Subscribe(AddNewByte);

            _messagesSubscription = _messagesObservable.SubscribeOn(TaskPoolScheduler.Default).Subscribe(OnNewMessage);
        }


        private void OnNewMessage(string message)
        {
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

        public Task Connect()
        {
            return _client.ConnectAsync();
        }


        public async Task<LoginResult> Login(string apiKey, string instanceName)
        {
            var request = new LoginRequest() {InstanceName = instanceName, ApiKey = apiKey};

            var t = _messagesObservable.FirstAsync().Select(ToLoginResult)
                .FirstAsync().Timeout(TimeSpan.FromSeconds(10))
                .Do(r => { _isAuthenticated = r.WasSuccessful; });


            var cts = new CancellationTokenSource();

            try
            {
                await _client.SendAsync(CreateRequest(request));
            }
            catch (Exception)
            {
                cts.Cancel();
                throw;
            }

            return await t.ToTask(cts.Token);
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


        private JObject CheckResponseStatus(string response)
        {
            var json = JObject.Parse(response);
            var result = json.SelectToken("result").Value<string>();
            if (result == null)
                throw new SyncanoException("Unexpected response: " + response);

            if (result == "NOK")
                throw new SyncanoException("Error: " + json.SelectToken("data").SelectToken("error").Value<string>());

            return json;
        }

        private  Task<T> SendCommandAsync<T>(ApiCommandRequest request, string contentToken)
        {
            return SendCommandAsync<T>(request, jo => jo.SelectToken("data").SelectToken(contentToken).ToObject<T>());
        }

        private async Task<T> SendCommandAsync<T>(ApiCommandRequest request, Func<JToken,T> getResult)
        {
            var t = _messagesObservable.Where(s => IsResponseToRequest(s, request))
                .Select(m =>
                {
                    try
                    {
                        JObject jo = CheckResponseStatus(m);
                        return getResult(jo);
                    }
                    catch (Exception e)
                    {
                        //Logger.Fatal("Failed to deserialize response: " + m, e);
                        throw;
                    }
                }).FirstAsync().Timeout(TimeSpan.FromSeconds(30)).Finally(() =>
                {
                    if (!_isDisposing)
                        _semaphore.Release();
                }
                );

            var cts = new CancellationTokenSource();

            try
            {
                await SendRequestAsync(request);
            }
            catch (Exception)
            {
                cts.Cancel();
                throw;
            }

            return await t.ToTask(cts.Token);
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
                        if (eachProperty.GetValue(parameters).GetType().IsConstructedGenericType && eachProperty.GetValue(parameters).GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
                        {
                            var dictionary = (Dictionary<string, string>)eachProperty.GetValue(parameters);
                            foreach (var item in dictionary)
                            {
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

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(8);
        private IDisposable _messagesSubscription;
        private bool _isDisposing;

        private async Task SendRequestAsync(ApiCommandRequest request)
        {
            if (_isAuthenticated == false) throw new SyncanoException("Cannot send any Requests when not logged in");


            await _semaphore.WaitAsync(TimeSpan.FromSeconds(30));
            try
            {
                await _client.SendAsync(CreateRequest(request));
            }


            catch (Exception)
            {
                if (!_isDisposing)
                    _semaphore.Release();
                throw;
            }
        }

        private bool IsResponseToRequest(string s, ApiCommandRequest request)
        {
            return s.Contains(string.Format(",\"message_id\":{0}}}", request.MessageId));
        }


        private byte[] CreateRequest(ISyncanoRequest request)
        {
            string cmd = JsonConvert.SerializeObject(request) + "\n";

            var messageBytes = Encoding.UTF8.GetBytes(cmd);
            if (messageBytes.Length > 128*1024)
                throw new SyncanoException("Request size exceeded, maximum size 128Kb");

            return messageBytes;
        }


        public void Dispose()
        {
            _isDisposing = true;
            if (_newBytesSubscription != null)
                _newBytesSubscription.Dispose();

            if (_messagesSubscription != null)
                _messagesSubscription.Dispose();

            _semaphore.Dispose();
            if (_client != null)
            {
                _client.Dispose();
            }
        }

        public Task<bool> GetAsync(string methodName, object parameters)
        {
            var request = CreateCommandRequest(methodName, parameters);
            return SendCommandAsync<bool>(request, jo => jo.SelectToken("result").Value<string>() == "OK");
        }

        public Task<T> GetAsync<T>(string methodName, string contentToken)
        {
            return GetAsync<T>(methodName, null, contentToken);
        }

        public Task<T> GetAsync<T>(string methodName, object parameters, string contentToken)
        {
            var request = CreateCommandRequest(methodName, parameters);
            return SendCommandAsync<T>(request, contentToken);
        }

        public Task<T> PostAsync<T>(string methodName, object parameters, string contentToken)
        {
            return GetAsync<T>(methodName, parameters, contentToken);
        }
    }
}