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

        public Task<bool> Activate(string projectId, string collectionId, bool force = false)
        {
            return _restClient.GetAsync("collection.activate",
                new {project_id = projectId, collection_id = collectionId, force = force});
        }

        public Task<bool> Deactivate(string projectId, string collectionId = null, string collectionKey = null)
        {
            if(collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("collection.deactivate",
                new {project_id = projectId, collection_id = collectionId, collection_key = collectionKey});
        }

        public Task<Collection> Update(string projectId, string collectionId = null, string collectionKey = null,
            string name = null, string description = null)
        {
            if(collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("collection.update",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    name = name,
                    description = description
                }, "collection", t => t.ToObject<Collection>());
        }

        public Task<bool> Delete(string projectId, string collectionId = null, string collectionKey = null)
        {
            if(collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("collection.delete",
                new {project_id = projectId, collection_id = collectionId, collection_key = collectionKey});
        }
    }
}
