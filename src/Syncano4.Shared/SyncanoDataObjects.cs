using System.Collections.Generic;
using Newtonsoft.Json;


#if dotNET
using System.Threading.Tasks;
#endif


namespace Syncano4.Shared
{
    public class SyncanoDataObjects<T>:SyncanoRepository<T,T> where T:DataObject
    {


        public SyncanoDataObjects(SyncanoClass syncanoClass, ISyncanoHttpClient httpClient)
            : base(syncanoClass.Links["objects"], httpClient)
        {
        }

     

        private static string ToJson(IList<FieldDef> schema)
        {
            //  var jsonSerializerSettings = new JsonSerializerSettings();
            // jsonSerializerSettings.Converters.Add(new StringEnumConverter() { CamelCaseText = true, AllowIntegerValues = true});

            var schemaJson = JsonConvert.SerializeObject(schema);
            return schemaJson;
        }


#if Unity3d
        public IList<T> List(int pageSize = 10)
        {
            return List(new Dictionary<string, object>() { {"page_size", pageSize }});
        }
       
#endif

#if dotNET
          public Task<IList<T>> ListAsync(int pageSize = 10)
        {
            return ListAsync(new Dictionary<string, object>() { {"page_size", pageSize }});
        }

#endif


    }
}