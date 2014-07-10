using Newtonsoft.Json;

namespace Syncano.Net
{
    public class Folder
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_custom")]
        public bool IsCustom { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("source_id")]
        public string SourceId { get; set; }
    }
}
