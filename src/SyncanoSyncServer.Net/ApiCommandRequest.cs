using System.Collections.Generic;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net
{
    public class ApiCommandRequest : ISyncanoRequest
    {
        public ApiCommandRequest(string method, long messageId)
        {
            Type = "call";
            this.Method = method;
            MessageId = messageId;
            this.Params = new Dictionary<string, object>();
        }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public Dictionary<string, object> Params { get; set; }

        [JsonProperty("message_id")]
        public long MessageId { get; set; }
    }
}