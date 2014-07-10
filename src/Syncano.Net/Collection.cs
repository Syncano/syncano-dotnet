using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Syncano.Net
{
    public class Collection
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(CollectionStatusEnumConverter))]
        public CollectionStatus Status { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }
    }
}
