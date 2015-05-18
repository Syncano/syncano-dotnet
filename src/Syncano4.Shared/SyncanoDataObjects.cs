using System.Collections.Generic;
using Newtonsoft.Json;
using Syncano4.Shared.Query;
using Syncano4.Shared.Serialization;
#if dotNET
using System.Threading.Tasks;

#endif

namespace Syncano4.Shared
{
    public class SyncanoDataObjects<T> : SyncanoRepository<T, T> where T : DataObject
    {
        public SyncanoDataObjects(InstanceResources instanceResources, string objectName, ISyncanoHttpClient httpClient)
            : base(i => i.Links["objects"], new SchemaLazyLinkProvider(instanceResources, objectName), httpClient)
        {
        }


#if Unity3d
        public IList<T> List(int pageSize = 10)
        {
            return List(new Dictionary<string, object>() {{"page_size", pageSize}});
        }

        public PageableResult<T> PageableList(int pageSize = 10)
        {
            var response = PageableList(new Dictionary<string, object>() {{"page_size", pageSize}});

            return new PageableResult<T>(this.HttpClient, response);
        }

        public PageableResult<T> PageableList(SyncanoQuery<T> query )
        {
            var response = PageableList(new Dictionary<string, object>() { { "query", query.ToJson() } });

            return new PageableResult<T>(this.HttpClient, response);
        }

#endif

#if dotNET
        public Task<IList<T>> ListAsync(int pageSize = 10)
        {
            return ListAsync(new Dictionary<string, object>() {{"page_size", pageSize}});
        }

        public async Task<PageableResult<T>> PageableListAsync(int pageSize = 10)
        {
            var response = await PageableListAsync(new Dictionary<string, object>() {{"page_size", pageSize}});

            return new PageableResult<T>(this.HttpClient, response);
        }

            public async Task<PageableResult<T>> PageableListAsync(SyncanoQuery<T> query)
        {
            var response = await PageableListAsync(new Dictionary<string, object>() {{"query", query.ToJson()}});

            return new PageableResult<T>(this.HttpClient, response);
        }

#endif

        public SyncanoQuery<T> CreateQuery()
        {
            return new SyncanoQuery<T>(this);
        }

        protected override IRequestContent ToRequestData(object requestObject)
        {
            return new JsonRequestContent(SyncanoJsonConverter.Serialize(requestObject)) ;
        }
    }
}