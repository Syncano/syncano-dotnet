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

          public async Task<PageableResult<T>> PageableListAsync(int pageSize = 10)
          {
              var response = await PageableListAsync(new Dictionary<string, object>() { { "page_size", pageSize } });

              return new PageableResult<T>(this.HttpClient, response);
          }

#endif


    }

    public class PageableResult<T>
    {
        private readonly ISyncanoHttpClient _syncanoHttpClient;
        private string _linkToNext;

        public PageableResult(ISyncanoHttpClient syncanoHttpClient, SyncanoResponse<T> response )

        {
            _syncanoHttpClient = syncanoHttpClient;
            this.Current = response.Objects;
            _linkToNext = response.Next;
        }

#if dotNET

        public async Task<PageableResult<T>> GetNext()
        {
          var response =  await   _syncanoHttpClient.ListAsync<T>(_linkToNext, null);

            return new PageableResult<T>(_syncanoHttpClient, response);
        }

#endif
        public IList<T> Current { get; private set; }

    }
}