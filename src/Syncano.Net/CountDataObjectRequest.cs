using System.Collections.Generic;

namespace Syncano.Net
{
    public class CountDataObjectRequest
    {
        public CountDataObjectRequest()
        {
            State = DataObjectState.All;
        }

        public string ProjectId { get; set; }

        public string CollectionId { get; set; }

        public string CollectionKey { get; set; }

        public DataObjectState State { get; set; }

        public string Folder { get; set; }

        public List<string> Folders { get; set; }

        public DataObjectContentFilter? Filter { get; set; }

        public string ByUser { get; set; }



    }
}
