using Newtonsoft.Json;

namespace SyncanoSyncServer.Net
{
    /// <summary>
    /// Login request send to Syncano in order to authenticate.
    /// </summary>
    public class LoginRequest : ISyncanoRequest
    {
        /// <summary>
        /// Api Key of Syncano Instance.
        /// </summary>
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        /// <summary>
        /// Name of Syncano Instance.
        /// </summary>
        [JsonProperty("instance")]
        public string InstanceName { get; set; }
    }
}