using System.Collections.Generic;

namespace Syncano.Net
{
    public class GetCollectionRequest
    {
        public GetCollectionRequest()
        {
            Status = CollectionStatus.All;
        }

        public string ProjectId { get; set; }

        public CollectionStatus Status { get; set; }

        public string Tag { get; set; }

        public List<string> Tags { get; set; }
    }
}
