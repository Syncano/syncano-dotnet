using Newtonsoft.Json;

namespace SyncanoSyncServer.Net
{
  public class LoginRequest : ISyncanoRequest
    {
        [JsonProperty("api_key")]
        public string ApiKey { get; set; }

        [JsonProperty("instance")]
        public string InstanceName { get; set; }
    }
}