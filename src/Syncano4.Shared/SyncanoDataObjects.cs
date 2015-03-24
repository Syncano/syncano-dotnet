using System.Collections.Generic;
using Newtonsoft.Json;


#if dotNet
using System.Threading.Tasks;
#endif


namespace Syncano4.Shared
{
    public class SyncanoDataObjects<T>:SyncanoRepository<T,T> where T:DataObject
    {


        public SyncanoDataObjects(SyncanoClass syncanoClass, ISyncanoHttpClient httpClient)
            : base(string.Format(@"/v1/instances/testinstance2/classes/{0}/objects/", syncanoClass.Name), httpClient)
        {
        }

     

        private static string ToJson(IList<SyncanoFieldSchema> schema)
        {
            //  var jsonSerializerSettings = new JsonSerializerSettings();
            // jsonSerializerSettings.Converters.Add(new StringEnumConverter() { CamelCaseText = true, AllowIntegerValues = true});

            var schemaJson = JsonConvert.SerializeObject(schema);
            return schemaJson;
        }


#if Unity3d
        public IList<T> Get(int pageSize = 10)
        {
            return Get(new Dictionary<string, object>() { {"page_size", pageSize }});
        }
       
#endif

#if dotNet
          public Task<IList<T>> GetAsync(int pageSize = 10)
        {
            return GetAsync(new Dictionary<string, object>() { {"page_size", pageSize }});
        }

#endif

      
    }
}