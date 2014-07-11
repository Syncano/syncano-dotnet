using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncano.Net
{
    public class CollectionRestClient
    {
        private readonly SyncanoRestClient _restClient;

        public CollectionRestClient(SyncanoRestClient restClient)
        {
            _restClient = restClient;
        }

        public Task<Collection> New(string projectId, string name, string key = null,
            string description = null)
        {
            return _restClient.GetAsync("collection.new",
                new { project_id = projectId, name = name, key = key, description = description }, "collection",
                t => t.ToObject<Collection>());
        }
    }
}
