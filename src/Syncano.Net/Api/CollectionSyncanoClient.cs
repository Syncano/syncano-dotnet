using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Syncano.Net.Data;
using Syncano.Net.Http;

namespace Syncano.Net.Api
{
    public class CollectionSyncanoClient
    {
        private readonly SyncanoHttpClient _httpClient;

        public CollectionSyncanoClient(SyncanoHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<Collection> New(string projectId, string name, string key = null,
            string description = null)
        {
            if(projectId == null || name == null)
                throw new ArgumentNullException();

            return _httpClient.PostAsync("collection.new",
                new { project_id = projectId, name, key,  description }, "collection",
                t => t.ToObject<Collection>());
        }

        public async Task<List<Collection>> Get(GetCollectionRequest request)
        {
            if(request.ProjectId == null)
                throw new ArgumentNullException();

            var withTags = request.Tags == null ? new List<string>() : new List<string>(request.Tags);
            if(request.Tag != null)
                withTags.Add(request.Tag);
            var withTagsArray = withTags.Count == 0 ? null : withTags.ToArray();

            return
                await
                    _httpClient.PostAsync("collection.get",
                        new {project_id = request.ProjectId, status = request.Status, with_tags = withTagsArray},
                        "collection", t => t.ToObject<List<Collection>>());
        }

        public Task<Collection> GetOne(string projectId, string collectionId = null, string collectionKey = null)
        {
            if(collectionId == null && collectionKey == null)
                throw  new ArgumentNullException();

            return _httpClient.GetAsync("collection.get_one",
                new {project_id = projectId, collection_id = collectionId, collection_key = collectionKey}, "collection",
                t => t.ToObject<Collection>());
        }

        public Task<bool> Activate(string projectId, string collectionId, bool force = false)
        {
            return _httpClient.GetAsync("collection.activate",
                new {project_id = projectId, collection_id = collectionId, force});
        }

        public Task<bool> Deactivate(string projectId, string collectionId = null, string collectionKey = null)
        {
            if(collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return _httpClient.GetAsync("collection.deactivate",
                new {project_id = projectId, collection_id = collectionId, collection_key = collectionKey});
        }

        public Task<Collection> Update(string projectId, string collectionId = null, string collectionKey = null,
            string name = null, string description = null)
        {
            if(collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return _httpClient.GetAsync("collection.update",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    name,
                    description
                }, "collection", t => t.ToObject<Collection>());
        }

        public Task<bool> Authorize(string apiClientId, Permissions permission, string projectId,
            string collectionId = null, string collectionKey = null)
        {
            if(collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            string permissionString = PermissionsParser.GetString(permission);

            return _httpClient.GetAsync("collection.authorize",
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

            return _httpClient.GetAsync("collection.deauthorize",
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

            return _httpClient.GetAsync("collection.delete",
                new {project_id = projectId, collection_id = collectionId, collection_key = collectionKey});
        }

        public Task<bool> AddTag(string projectId, string tag, string collectionId = null, string collectionKey = null, 
            double weight = 1.0, bool removeOther = false)
        {
            if(collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return _httpClient.GetAsync("collection.add_tag",
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

            return _httpClient.GetAsync("collection.add_tag",
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

            return _httpClient.GetAsync("collection.delete_tag",
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

            return _httpClient.GetAsync("collection.delete_tag",
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
