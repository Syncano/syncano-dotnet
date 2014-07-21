using System.Collections.Generic;
using Syncano.Net.Data;

namespace Syncano.Net
{
    /// <summary>
    /// Request for counting DataObjects.
    /// </summary>
    public class CountDataObjectRequest
    {
        public CountDataObjectRequest()
        {
            State = DataObjectState.All;
        }

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
        /// State of data to be counted. Accepted values: Pending, Moderated, All. Default value: All.
        /// </summary>
        public DataObjectState State { get; set; }

        /// <summary>
        /// Folder name that data will be counted from. If not presents counts data from across all collection folders. Max 100 values per request.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// Folder names that data will be counted from. If not presents counts data from across all collection folders. Max 100 values per request.
        /// </summary>
        public List<string> Folders { get; set; }

        /// <summary>
        /// Filtering by content.
        /// </summary>
        public DataObjectContentFilter? Filter { get; set; }

        /// <summary>
        /// If specified, filter by user name.
        /// </summary>
        public string ByUser { get; set; }



    }
}
