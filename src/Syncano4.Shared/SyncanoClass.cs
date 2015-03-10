using Newtonsoft.Json;

namespace Syncano4.Shared
{
    public class SyncanoClass
    {
        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("description")]
        public string Description { get; set; }

        
        [JsonProperty("objects_count")]
        public string ObjectsCount { get; set; }

    }
}