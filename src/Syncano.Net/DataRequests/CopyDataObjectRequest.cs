using System.Collections.Generic;

namespace Syncano.Net.DataRequests
{
    /// <summary>
    /// Request for copying DataObjects.
    /// </summary>
    public class CopyDataObjectRequest
    {
        /// <summary>
        /// Project id.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// Collection id defining collection containing data.
        /// </summary>
        public string CollectionId { get; set; }

        /// <summary>
        /// Collection key defining collection containing data.
        /// </summary>
        public string CollectionKey { get; set; }

        /// <summary>
        /// Data id.
        /// </summary>
        public string DataId { get; set; }

        /// <summary>
        /// Dataids. Max 100 ids per request.
        /// </summary>
        public List<string> DataIds { get; set; }
    }
}
