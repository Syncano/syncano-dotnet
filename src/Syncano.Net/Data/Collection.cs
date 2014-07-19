using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Syncano.Net.Data
{
    /// <summary>
    /// Collection object.
    /// </summary>
    public class Collection
    {
        /// <summary>
        /// Collection id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Collection status.
        /// </summary>
        [JsonProperty("status")]
        [JsonConverter(typeof(CollectionStatusEnumConverter))]
        public CollectionStatus Status { get; set; }

        /// <summary>
        /// Collection name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Collection description - if set.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Collection key - if set.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Date and time when collection became active - empty for new collections.
        /// </summary>
        [JsonProperty("start_date")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Date and time when collection became inactive - empty for active collections.
        /// </summary>
        [JsonProperty("end_date")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        ///  Consisting of arbitrary tags defined as name (string) - weight (decimal) pairs.
        /// </summary>
        [JsonProperty("tags")]
        public Dictionary<string, double> Tags { get; set; }
    }
}
