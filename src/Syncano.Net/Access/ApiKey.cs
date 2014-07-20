using Newtonsoft.Json;

namespace Syncano.Net.Access
{
    /// <summary>
    /// Class representing Api Key.
    /// </summary>
    public class ApiKey
    {
        /// <summary>
        /// API client id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// API client type.
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(ApiKeyTypeEnumConverter))]
        public ApiKeyType Type { get; set; }

        /// <summary>
        /// API client description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// API key.
        /// </summary>
        [JsonProperty("api_key")]
        public string ApiKeyValue { get; set; }

        /// <summary>
        /// Api Key role.
        /// </summary>
        [JsonProperty("role")]
        public Role Role { get; set; }
    }
}
