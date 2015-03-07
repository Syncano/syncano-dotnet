using System.Collections.Generic;
using Newtonsoft.Json;

namespace Syncano4.Shared
{
    public class SyncanoResponse<T>
    {
        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("objects")]
        public IList<T> Objects { get; set; }

        [JsonProperty("prev")]
        public string Prev { get; set; }

    }
}