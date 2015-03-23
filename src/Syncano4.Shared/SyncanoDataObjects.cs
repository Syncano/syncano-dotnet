using System.Collections.Generic;
using Newtonsoft.Json;


#if dotNet
using System.Threading.Tasks;
#endif


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
        public IList<T> Get<T>(int pageSize = 10)
        {
            return _httpClient.Get<T>(_link, new Dictionary<string, object>() { {"page_size", pageSize }});
        }

        public T Add<T>(T dataObject)
        {
            
             var jsonString = JsonConvert.SerializeObject(dataObject);
            var objectAsDictionary =  JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
            return _httpClient.Post<T>(_link, objectAsDictionary);
        }

       
#endif

#if dotNet
          public Task<IList<T>> GetAsync<T>(int pageSize = 10)
        {
            return _httpClient.GetAsync<T>(_link, new Dictionary<string, object>() { {"page_size", pageSize }});
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