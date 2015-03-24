using System.Collections.Generic;

namespace Syncano4.Shared
{
    public class CreateInstanceArgs : IArgs
    {
        public string Name { get; set; }

        public string Description { get; set; }
        
        public IDictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>() { { "name", this.Name }, { "description", this.Description } };
        }
    }
}