using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Syncano.Net.Data;

namespace Syncano.Net.Api
{
    public class CollectionSyncanoClient
    {
        private readonly ISyncanoClient _syncanoClient;

        public CollectionSyncanoClient(ISyncanoClient syncanoClient)
        {
            _syncanoClient = syncanoClient;
        }

        /// <summary>
        /// Create a new collection within the specified project.
        /// </summary>
        /// <param name="projectId">Project id that the collection will be created for.</param>
        /// <param name="name">New collection's name.</param>
        /// <param name="key">New collection's key.</param>
        /// <param name="description">New collection's description.</param>
        /// <returns>New Collection object.</returns>
        public Task<Collection> New(string projectId, string name, string key = null,
            string description = null)
        {
            if(projectId == null || name == null)
                throw new ArgumentNullException();

            return _syncanoClient.PostAsync<Collection>("collection.new",
                new { project_id = projectId, name, key,  description }, "collection");
        }

        /// <summary>
        /// Get collections by a specified request.
        /// </summary>
        /// <param name="request">Query request.</param>
        /// <returns>List of Collection objects.</returns>
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
                    _syncanoClient.PostAsync<List<Collection>>("collection.get",
                        new {project_id = request.ProjectId, status = request.Status.ToString(), with_tags = withTagsArray},
                        "collection");
        }

        /// <summary>
        /// Get one collection from a specified project.
        /// <remarks>The collection_id/collection_key parameter means that one can use either one of them - collection_id or collection_key.</remarks>
        /// <remarks>User API key usage permitted if read_data permission is added to specified collection through collection.authorize() or project.authorize().</remarks>
        /// </summary>
        /// <param name="projectId">Project id.</param>
        /// <param name="collectionId">Collection id defining collection.</param>
        /// <param name="collectionKey">Collection key defining collection.</param>
        /// <returns>Collection object.</returns>
        public Task<Collection> GetOne(string projectId, string collectionId = null, string collectionKey = null)
        {
            if(collectionId == null && collectionKey == null)
                throw  new ArgumentNullException();

            if(projectId == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync<Collection>("collection.get_one",
                new {project_id = projectId, collection_id = collectionId, collection_key = collectionKey}, "collection");
        }

        /// <summary>
        /// Activates specified collection.
        /// </summary>
        /// <param name="projectId">Project id.</param>
        /// <param name="collectionId">Collection id defining collection to be activated.</param>
        /// <param name="force">If set to True, will force the activation by deactivating all other collections that may share it's data_key.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> Activate(string projectId, string collectionId, bool force = false)
        {
            if(projectId == null || collectionId == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("collection.activate",
                new {project_id = projectId, collection_id = collectionId, force});
        }

        /// <summary>
        /// Deactivates specified collection.
        /// <remarks>The collection_id/collection_key parameter means that one can use either one of them - collection_id or collection_key.</remarks>
        /// </summary>
        /// <param name="projectId">Project id.</param>
        /// <param name="collectionId">Collection id defining collection to be deactivated.</param>
        /// <param name="collectionKey">Collection key defining collection to be deactivated.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> Deactivate(string projectId, string collectionId = null, string collectionKey = null)
        {
            if(collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if(projectId == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("collection.deactivate",
                new {project_id = projectId, collection_id = collectionId, collection_key = collectionKey});
        }

        /// <summary>
        /// Update existing collection.
        /// <remarks>If both the id and key are specified, will use id for getting collection while collection_key will be updated with a new value.</remarks>
        /// </summary>
        /// <param name="projectId">Project id.</param>
        /// <param name="collectionId">Collection id defining a collection.</param>
        /// <param name="collectionKey">Collection key defining a collection.</param>
        /// <param name="name">New collection name.</param>
        /// <param name="description">New collection description.</param>
        /// <returns>Updated Collection object.</returns>
        public Task<Collection> Update(string projectId, string collectionId = null, string collectionKey = null,
            string name = null, string description = null)
        {
            if(collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if(projectId == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync<Collection>("collection.update",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    name,
                    description
                }, "collection");
        }

        /// <summary>
        /// Adds container-level permission to specified User API client. Requires Backend API key with Admin permission role.
        /// <remarks>The collection_id/collection_key parameter means that one can use either one of them - collection_id or collection_key.</remarks>
        /// </summary>
        /// <param name="apiClientId">User API client id.</param>
        /// <param name="permission">User API client's permission to add.</param>
        /// <param name="projectId">Defines project containing specified container.</param>
        /// <param name="collectionId">Collection id defining collection that permission will be added to.</param>
        /// <param name="collectionKey">Collection key defining collection that permission will be added to.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> Authorize(string apiClientId, Permissions permission, string projectId,
            string collectionId = null, string collectionKey = null)
        {
            if(collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if(apiClientId == null || projectId == null)
                throw new ArgumentNullException();

            string permissionString = PermissionsParser.GetString(permission);

            return _syncanoClient.GetAsync("collection.authorize",
                new
                {
                    api_client_id = apiClientId,
                    permission = permissionString,
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey
                });
        }

        /// <summary>
        /// Removes container-level permission from specified User API client. Requires Backend API key with Admin permission role.
        /// <remarks>The collection_id/collection_key parameter means that one can use either one of them - collection_id or collection_key.</remarks>
        /// </summary>
        /// <param name="apiClientId">User API client id.</param>
        /// <param name="permission">User API client's permission to add.</param>
        /// <param name="projectId">Defines project containing specified container.</param>
        /// <param name="collectionId">Collection id defining collection that permission will be added to.</param>
        /// <param name="collectionKey">Collection key defining collection that permission will be added to.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> Deauthorize(string apiClientId, Permissions permission, string projectId,
            string collectionId = null, string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if (apiClientId == null || projectId == null)
                throw new ArgumentNullException();

            string permissionString = PermissionsParser.GetString(permission);

            return _syncanoClient.GetAsync("collection.deauthorize",
                new
                {
                    api_client_id = apiClientId,
                    permission = permissionString,
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey
                });
        }

        /// <summary>
        /// Permanently delete a specified collection and all associated data.
        /// <remarks>The collection_id/collection_key parameter means that one can use either one of them - collection_id or collection_key.</remarks>
        /// </summary>
        /// <param name="projectId">Project id.</param>
        /// <param name="collectionId">Collection id defining the collection to be deleted.</param>
        /// <param name="collectionKey">Collection key defining the collection to be deleted.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> Delete(string projectId, string collectionId = null, string collectionKey = null)
        {
            if(collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if(projectId == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("collection.delete",
                new {project_id = projectId, collection_id = collectionId, collection_key = collectionKey});
        }

        /// <summary>
        /// Add a tag to specific event.
        /// <remarks>Note: tags are case sensitive.</remarks>
        /// </summary>
        /// <param name="request">Tags manage request.</param>
        /// <param name="weight">Tag(s) weight. Default value: 1.</param>
        /// <param name="removeOther">If true, will remove all other tags of specified collection. Default value: False.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> AddTag(ManageCollactionTagsRequest request, double weight = 1.0, bool removeOther = false)
        {
            if(request.CollectionId == null && request.CollectionKey == null)
                throw new ArgumentNullException();

            if(request.ProjectId == null)
                throw new ArgumentNullException();

            var tagsList = request.Tags == null ? new List<string>() : new List<string>(request.Tags);
            if (request.Tag != null)
                tagsList.Add(request.Tag);
            var tags = tagsList.Count == 0 ? null : tagsList.ToArray();

            if (tags == null)
                throw new ArgumentException();

            if(tags.Any(t => t == ""))
                throw new ArgumentException();

            return _syncanoClient.GetAsync("collection.add_tag",
                new
                {
                    project_id = request.ProjectId,
                    collection_id = request.CollectionId,
                    collection_key = request.CollectionKey,
                    tags = tags.ToArray(),
                    weight = weight.ToString(CultureInfo.InvariantCulture),
                    remove_other = removeOther
                });
        }

        /// <summary>
        /// Delete a tag or tags from specified collection.
        /// <remarks>Note: tags are case sensitive.</remarks>
        /// </summary>
        /// <param name="request">Tags manage request.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> DeleteTag(ManageCollactionTagsRequest request)
        {
            if (request.CollectionId == null && request.CollectionKey == null)
                throw new ArgumentNullException();

            if (request.ProjectId == null)
                throw new ArgumentNullException();

            var tagsList = request.Tags == null ? new List<string>() : new List<string>(request.Tags);
            if (request.Tag != null)
                tagsList.Add(request.Tag);
            var tags = tagsList.Count == 0 ? null : tagsList.ToArray();

            if (tags == null)
                throw new ArgumentException();

            if (tags.Any(t => t == ""))
                throw new ArgumentException();

            return _syncanoClient.GetAsync("collection.delete_tag",
                new
                {
                    project_id = request.ProjectId,
                    collection_id = request.CollectionId,
                    collection_key = request.CollectionKey,
                    tags,
                });
        }
    }
}
