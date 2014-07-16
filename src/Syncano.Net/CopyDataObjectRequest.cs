using System.Collections.Generic;

namespace Syncano.Net
{
    public class CopyDataObjectRequest
    {
        public string ProjectId { get; set; }

        public string CollectionId { get; set; }

        public string CollectionKey { get; set; }

        public string DataId { get; set; }

        public List<string> DataIds { get; set; }
    }
}
