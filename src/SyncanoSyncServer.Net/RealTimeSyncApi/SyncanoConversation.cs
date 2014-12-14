using System;
using System.Reactive.Subjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Syncano.Net;

namespace SyncanoSyncServer.Net.RealTimeSyncApi
{
    public class SyncanoConversation<T> : ISyncanoConversation
    {
        private readonly Func<JToken, T> _responseMapperFunc;

        public SyncanoConversation(ApiCommandRequest request, Func<JToken, T> responseMapperFunc)
        {
            _responseMapperFunc = responseMapperFunc;
            Request = request;
            _id = request.MessageId.ToString();
            _responseSubject = new Subject<T>();
            Created = DateTime.UtcNow;
        }

        public string Id
        {
            get { return _id; }
        }

        public bool WasTimeouted { get; private set; }
        public ApiCommandRequest Request { get; private set; }

        public TimeSpan Duration
        {
            get
            {
                if (this.Finished.HasValue && this.Sent.HasValue)
                    return this.Finished.Value - this.Sent.Value;
                else if(this.Sent.HasValue)
                    return DateTime.UtcNow - this.Sent.Value;
                else if (this.Finished.HasValue && !this.Sent.HasValue)
                    return this.Finished.Value - this.Created;
                else
                    return DateTime.UtcNow - this.Created;

            }
        }

        public DateTime Created { get; private set; }

        public DateTime? Finished { get; private set; }

        public IObservable<T> ResponseObservable
        {
            get { return _responseSubject; }
        }

        public DateTime? Sent { get; private set; }

        public bool HasCompleted
        {
            get { return Finished.HasValue; }
        }

        private Subject<T> _responseSubject;

        public void SetResponse(string message)
        {

            var json = JObject.Parse(message);

            var messageResult = json.SelectToken("result").Value<string>();

            if (messageResult == null)
            {
                SetError(new SyncanoException("Unexpected response: " + message));
                return;
            }

            if (messageResult == "NOK")
            {
                SetError(new SyncanoException("Error: " + json.SelectToken("data").SelectToken("error").Value<string>()));
                return;
            }


            T result = _responseMapperFunc(json);

            _responseSubject.OnNext(result);
            this.Finished = DateTime.UtcNow;
            _responseSubject.OnCompleted();
            
        }
        
        public void SetError(Exception exception)
        {
            this.Finished = DateTime.UtcNow;
            _responseSubject.OnError(exception);
        }


        private static readonly TimeSpan _timeoutDuration = TimeSpan.FromSeconds(30);
        private string _id;

        public void VerifyTimeout()
        {
            if (!HasCompleted)
            {
                if (DateTime.UtcNow - Sent > _timeoutDuration)
                {
                    SetError(new SyncanoException(String.Format("Timeout. Response was not received after: {0}s", _timeoutDuration.TotalSeconds)));
                    this.WasTimeouted = true;
                }
            }
        }

        public void SetSent()
        {
            this.Sent = DateTime.UtcNow;
        }
    }
}