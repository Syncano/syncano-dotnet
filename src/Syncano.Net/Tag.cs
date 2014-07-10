using Newtonsoft.Json;

namespace Syncano.Net
{
    public class Tag
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("weight")]
        public decimal Weight { get; set; }
    }
}
