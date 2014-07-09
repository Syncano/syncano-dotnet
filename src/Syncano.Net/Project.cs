using Newtonsoft.Json;

namespace Syncano.Net
{
    public class Project
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

    }
}