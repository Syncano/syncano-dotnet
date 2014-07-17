using Newtonsoft.Json;

namespace Syncano.Net.Data
{
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nick")]
        public string Nick { get; set; }

        [JsonProperty("avatar")]
        public Avatar Avatar { get; set; }
    }
}
