using System.Collections.Generic;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net
{
    /// <summary>
    /// Request for Syncano Api Command.
    /// </summary>
    public class ApiCommandRequest : ISyncanoRequest
    {
        /// <summary>
        /// Creates ApiCommandRequest object.
        /// </summary>
        /// <param name="method">Method.</param>
        /// <param name="messageId">Message Id.</param>
        public ApiCommandRequest(string method, long messageId)
        {
            Type = "call";
            Method = method;
            MessageId = messageId;
            Params = new Dictionary<string, object>();
        }

        /// <summary>
        /// Type.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Method.
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; }

        /// <summary>
        /// Parameters of request.
        /// </summary>
        [JsonProperty("params")]
        public Dictionary<string, object> Params { get; set; }

        /// <summary>
        /// Message Id.
        /// </summary>
        [JsonProperty("message_id")]
        public long MessageId { get; set; }
    }
}