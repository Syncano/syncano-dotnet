using System.Collections.Generic;
using Newtonsoft.Json;

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

#endif
    }
}