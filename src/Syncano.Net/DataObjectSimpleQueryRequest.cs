using System.Collections.Generic;
using Syncano.Net.Api;
using Syncano.Net.Data;

namespace Syncano.Net
{
    public class DataObjectSimpleQueryRequest
    {
        public DataObjectSimpleQueryRequest()
        {
            State = DataObjectState.All;
            Limit = DataObjectSyncanoClient.MaxVauluesPerRequest;
            Filter = null;
        }

        public string ProjectId { get; set; }

        public string CollectionId { get; set; }

        public string CollectionKey { get; set; }

        public string DataId { get; set; }

        public List<string> DataIds { get; set; }

        public DataObjectState State { get; set; }

        public string Folder { get; set; }

        public List<string> Folders { get; set; }

        public DataObjectContentFilter? Filter { get; set; }

        public string ByUser { get; set; }

        public int Limit { get; set; }
    }
}
