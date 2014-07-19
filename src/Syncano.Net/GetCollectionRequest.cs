using System.Collections.Generic;
using Syncano.Net.Data;

namespace Syncano.Net
{
    /// <summary>
    /// Request for querying collections.
    /// </summary>
    public class GetCollectionRequest
    {
        public GetCollectionRequest()
        {
            Status = CollectionStatus.All;
        }

        /// <summary>
        /// Project id.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// Status of events to list. Accepted values: active, inactive, all. Default value: all.
        /// </summary>
        public CollectionStatus Status { get; set; }

        /// <summary>
        /// If specified, will only list events that has specified tag defined. Note: tags are case sensitive.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// If specified, will only list events that has specified tags defined. Note: tags are case sensitive.
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
