using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Syncano4.Shared
{
    public class SyncanoDataObjects
    {
        private readonly string _link;
        private readonly ISyncanoHttpClient _httpClient;

        public SyncanoDataObjects(SyncanoClass syncanoClass, ISyncanoHttpClient httpClient)
        {
            _link = string.Format(@"/v1/instances/testinstance2/classes/{0}/objects/", syncanoClass.Name);
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

        public T Add<T>(T dataObject)
        {
            
             var jsonString = JsonConvert.SerializeObject(dataObject);
            var objectAsDictionary =  JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
            return _httpClient.Post<T>(_link, objectAsDictionary);
        }

       
#endif

#if dotNet
        public Task<IList<SyncanoClass>> GetAsync()
        {
            return _httpClient.GetAsync<SyncanoClass>(_link, null);
        }

        public Task<T> AddAsync<T>(T dataObject)
        {
            var jsonString = JsonConvert.SerializeObject(dataObject);
            var objectAsDictionary =  JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
            return _httpClient.PostAsync<T>(_link, objectAsDictionary);
        }


#endif
    }
}