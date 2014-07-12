using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public async Task<List<Collection>> Get(string projectId, CollectionStatus status = CollectionStatus.All)
        {
            return await _restClient.GetAsync("collection.get",
                new { project_id = projectId, status = status }, "collection",
                t => t.ToObject<List<Collection>>());
        }

        public async Task<List<Collection>> Get(string projectId, string withTag, CollectionStatus status = CollectionStatus.All)
        {
            if(withTag == null)
                throw new ArgumentNullException();

            return await _restClient.GetAsync("collection.get",
                new {project_id = projectId, status = status, with_tags = withTag}, "collection",
                t => t.ToObject<List<Collection>>());
        }

        public async Task<List<Collection>> Get(string projectId, IEnumerable<string> withTags, CollectionStatus status = CollectionStatus.All)
        {
            if(withTags == null)
                throw new ArgumentNullException();

            return await _restClient.GetAsync("collection.get",
                        new {project_id = projectId, status = status, with_tags = withTags.ToArray()}, "collection", t => t.ToObject<List<Collection>>());
        }

        public Task<Collection> GetOne(string projectId, string collectionId = null, string collectionKey = null)
        {
            if(collectionId == null && collectionKey == null)
                throw  new ArgumentNullException();

            return _restClient.GetAsync("collection.get_one",
                new {project_id = projectId, collection_id = collectionId, collection_key = collectionKey}, "collection",
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

        public Task<bool> Authorize(string apiClientId, Permissions permission, string projectId,
            string collectionId = null, string collectionKey = null)
        {
            if(collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            string permissionString = PermissionsParser.GetString(permission);

            return _restClient.GetAsync("collection.authorize",
                new
                {
                    api_client_id = apiClientId,
                    permission = permissionString,
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey
                });
        }

        public Task<bool> Deauthorize(string apiClientId, Permissions permission, string projectId,
            string collectionId = null, string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            string permissionString = PermissionsParser.GetString(permission);

            return _restClient.GetAsync("collection.deauthorize",
                new
                {
                    api_client_id = apiClientId,
                    permission = permissionString,
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey
                });
        }

        public Task<bool> Delete(string projectId, string collectionId = null, string collectionKey = null)
        {
            if(collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("collection.delete",
                new {project_id = projectId, collection_id = collectionId, collection_key = collectionKey});
        }

        public Task<bool> AddTag(string projectId, string tag, string collectionId = null, string collectionKey = null, 
            double weight = 1.0, bool removeOther = false)
        {
            if(collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("collection.add_tag",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    tags = tag,
                    weight = weight.ToString(CultureInfo.InvariantCulture),
                    remove_other = removeOther
                });
        }

        public Task<bool> AddTag(string projectId, IEnumerable<string> tags, string collectionId = null, string collectionKey = null,
            double weight = 1.0, bool removeOther = false)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("collection.add_tag",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    tags = tags.ToArray(),
                    weight = weight.ToString(CultureInfo.InvariantCulture),
                    remove_other = removeOther
                });
        }

        public Task<bool> DeleteTag(string projectId, string tag, string collectionId = null, string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null || tag == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("collection.delete_tag",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    tags = tag,
                });
        }

        public Task<bool> DeleteTag(string projectId, IEnumerable<string> tags, string collectionId = null, string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null || tags == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("collection.delete_tag",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    tags = tags.ToArray(),
                });
        }
    }
}
