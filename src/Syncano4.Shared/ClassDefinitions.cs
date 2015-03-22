using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
#if dotNet
using System.Threading.Tasks;

#endif

namespace Syncano4.Shared
{
    public class ClassDefinitions
    {
        private readonly string _link;
        private readonly ISyncanoHttpClient _httpClient;

        public ClassDefinitions(string link, ISyncanoHttpClient httpClient)
        {
            _link = link;
            _httpClient = httpClient;
        }

        private static string ToJson(IList<SyncanoFieldSchema> schema)
        {
          //  var jsonSerializerSettings = new JsonSerializerSettings();
           // jsonSerializerSettings.Converters.Add(new StringEnumConverter() { CamelCaseText = true, AllowIntegerValues = true});

            var schemaJson = JsonConvert.SerializeObject(schema);
            return schemaJson;
        }


#if Unity3d
        public IList<SyncanoClass> Get()
        {
            return _httpClient.Get<SyncanoClass>(_link, null);
        }

        public SyncanoClass Add(string name, string description, IList<SyncanoFieldSchema> schema)
        {
            
            var parameters = new Dictionary<string, object>() { { "name", name }, { "description", description }, {"schema", ToJson(schema)} };
            return _httpClient.Post<SyncanoClass>(_link, parameters);
        }

       
#endif

#if dotNet
        public Task<IList<SyncanoClass>> GetAsync()
        {
            return _httpClient.GetAsync<SyncanoClass>(_link, null);
        }

        public Task<SyncanoClass> AddAsync(string name, string description, IList<SyncanoFieldSchema> schema)
        {
            var parameters = new Dictionary<string, object>() {{"name", name}, {"description", description}, {"schema", ToJson(schema)}};
            return _httpClient.PostAsync<SyncanoClass>(_link, parameters);
        }


#endif
    }
}