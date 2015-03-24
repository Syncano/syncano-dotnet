using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
#if dotNet
using System.Threading.Tasks;

#endif

namespace Syncano4.Shared
{
    public class CreateSyncanoClassArgs : IArgs
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IList<SyncanoFieldSchema> Schema {get; set; }

        public IDictionary<string, object> ToDictionary()
        {
             var schemaJson = JsonConvert.SerializeObject(this.Schema);
            return new Dictionary<string, object>() { { "name", this.Name }, { "description", this.Description }, {"schema", schemaJson} };
        }
    }

    public class ClassDefinitions : SyncanoRepository<SyncanoClass, CreateSyncanoClassArgs>
    {
        public ClassDefinitions(string link, ISyncanoHttpClient httpClient) : base(link, httpClient)
        {
        }
    }

}