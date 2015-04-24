using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
#if dotNET
using System.Threading.Tasks;

#endif

namespace Syncano4.Shared
{
    public class NewClass 
    {

        public static NewClass From<T>()
        {
            var schema = SchemaMapping.GetSchema<T>();
            return new NewClass(typeof (T).Name, schema.ToArray());
        }

        public NewClass()
        {
            
        }
        public NewClass(string name, params FieldDef[] fields)
        {
            this.Name = name;

            if (fields != null)
                this.Schema = new List<FieldDef>(fields);
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("schema")]
        public IList<FieldDef> Schema { get; set; }
        
    }

    public class ClassDefinitions : SyncanoRepository<SyncanoClass, NewClass>
    {
        public ClassDefinitions(Func<ILazyLinkProvider, string> getLink, InstanceLazyLinkProvider instanceLazyLinkProvider, ISyncanoHttpClient httpClient)
            : base(getLink, instanceLazyLinkProvider, httpClient)
        {
        }
    }
}