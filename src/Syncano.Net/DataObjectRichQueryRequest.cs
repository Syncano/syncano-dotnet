using System.Collections.Generic;

namespace Syncano.Net
{
    public class DataObjectRichQueryRequest : DataObjectSimpleQueryRequest
    {
        public DataObjectRichQueryRequest()
        {
            Order = DataObjectOrder.Ascending;
            OrderBy = DataObjectOrderBy.CreatedAt;
            ChildrenLimit = 100;
        }

        public string Since { get; set; }

        public string MaxId { get; set; }

        public DataObjectOrder Order { get; set; }

        public DataObjectOrderBy OrderBy { get; set; }

        public bool IncludeChildren { get; set; }

        public int Depth { get; set; }

        public int ChildrenLimit { get; set; }

        public string ParentId { get; set; }

        public List<string> ParentIds { get; set; }

        public string ChildId { get; set; }

        public List<string> ChildIds { get; set; }


    }
}
