using Newtonsoft.Json;

namespace Syncano.Net.Access
{
    public class ApiKey
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(ApiKeyTypeEnumConverter))]
        public ApiKeyType Type { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("api_key")]
        public string ApiKeyValue { get; set; }

        [JsonProperty("role")]
        public Role Role { get; set; }
    }
}
