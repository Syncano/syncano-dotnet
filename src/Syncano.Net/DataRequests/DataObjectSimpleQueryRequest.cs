using System.Collections.Generic;
using Syncano.Net.Api;
using Syncano.Net.Data;

namespace Syncano.Net.DataRequests
{
    /// <summary>
    /// Request for querying DataObjects.
    /// </summary>
    public class DataObjectSimpleQueryRequest
    {
        /// <summary>
        /// Creates DataObjectSimpleQueryRequest object.
        /// </summary>
        public DataObjectSimpleQueryRequest()
        {
            State = DataObjectState.All;
            Limit = DataObjectSyncanoClient.MaxVauluesPerRequest;
            Filter = null;
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
        /// If specified, will filter by Data id.
        /// </summary>
        public string DataId { get; set; }

        /// <summary>
        /// If specified, will filter by Data ids. Max 100 ids per request.
        /// </summary>
        public List<string> DataIds { get; set; }

        /// <summary>
        /// If specified, filter by Data state. Accepted values: Pending, Moderated, All. Default value: All.
        /// </summary>
        public DataObjectState State { get; set; }

        /// <summary>
        /// If specified, filter by specified folder.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// If specified, filter by specified folders. Max 100 values per request.
        /// </summary>
        public List<string> Folders { get; set; }

        /// <summary>
        /// If specified filtering by content.
        /// </summary>
        public DataObjectContentFilter? Filter { get; set; }

        /// <summary>
        /// If specified, filter by user name.
        /// </summary>
        public string ByUser { get; set; }

        /// <summary>
        /// Number of Data Objects to process. Default and max value: 100.
        /// </summary>
        public int Limit { get; set; }
    }
}
