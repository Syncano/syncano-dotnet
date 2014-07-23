using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncano.Net.Data;
namespace Syncano.Net.Api
{
    /// <summary>
    /// Class with Folder management api.
    /// </summary>
    public class FolderSyncanoClient
    {
        private readonly ISyncanoClient _syncanoClient;

        /// <summary>
        /// Creates FolderSyncanoClient object.
        /// </summary>
        /// <param name="syncanoClient">Object implementing ISyncanoClient interface. Provides means for connecting to Syncano.</param>
        public FolderSyncanoClient(ISyncanoClient syncanoClient)
        {
            _syncanoClient = syncanoClient;
        }

        /// <summary>
        /// Create new folder within a specified collection.
        /// </summary>
        /// <remarks>The collection_id/collection_key parameter means that one can use either one of them - collection_id or collection_key.</remarks>
        /// <param name="projectId">Project id.</param>
        /// <param name="name">Folder name.</param>
        /// <param name="collectionId">Collection id defining collection where folder will be created.</param>
        /// <param name="collectionKey">Collection key defining collection where folder will be created.</param>
        /// <returns>New Folder object.</returns>
        public Task<Folder> New(string projectId, string name, string collectionId = null,
            string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if(projectId == null || name == null)
                throw new ArgumentNullException();

            return _syncanoClient.PostAsync<Folder>("folder.new", new { project_id = projectId, collection_id = collectionId, collection_key = collectionKey, name },
                "folder");
        }

        /// <summary>
        /// Get folders for a specified collection.
        /// <remarks>The collection_id/collection_key parameter means that one can use either one of them - collection_id or collection_key.</remarks>
        /// <remarks>User API key usage permitted. Returns only collections that have``read_data`` permission added through folder.authorize(), collection.authorize() or project.authorize().</remarks>
        /// </summary>
        /// <param name="projectId">Project id.</param>
        /// <param name="collectionId">Collection id defining the collection for which folders will be returned.</param>
        /// <param name="collectionKey">Collection key defining the collection for which folders will be returned.</param>
        /// <returns>List of Folder objects.</returns>
        public async Task<List<Folder>> Get(string projectId, string collectionId = null,
            string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if(projectId == null)
                throw new ArgumentNullException();

            return await _syncanoClient.GetAsync<List<Folder>>("folder.get", new { project_id = projectId, collection_id = collectionId, collection_key = collectionKey }, "folder");
        }

        /// <summary>
        /// Get folder for a specified collection and folder name.
        /// <remarks>The collection_id/collection_key parameter means that one can use either one of them - collection_id or collection_key.</remarks>
        /// <remarks>User API key usage permitted if read_data permission is added to specified folder through folder.authorize(), collection.authorize() or project.authorize().</remarks>
        /// </summary>
        /// <param name="projectId">Project id.</param>
        /// <param name="folderName">Folder name defining folder.</param>
        /// <param name="collectionId">Collection id defining a collection for which the folder will be returned.</param>
        /// <param name="collectionKey">Collection key defining a collection for which the folder will be returned.</param>
        /// <returns>Folder object.</returns>
        public Task<Folder> GetOne(string projectId, string folderName, string collectionId = null,
            string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if(projectId == null || folderName == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync<Folder>("folder.get_one", new { project_id = projectId, collection_id = collectionId, collection_key = collectionKey, folder_name = folderName }, "folder");
        }

        /// <summary>
        /// Create existing folder.
        /// <remarks>The collection_id/collection_key parameter means that one can use either one of them - collection_id or collection_key.</remarks>
        /// </summary>
        /// <param name="projectId">Project id.</param>
        /// <param name="name">Current folder name.</param>
        /// <param name="collectionId">Collection id defining collection where folder exists.</param>
        /// <param name="collectionKey">Collection key defining collection where folder exists.</param>
        /// <param name="newName">New folder name.</param>
        /// <param name="sourceId">New source id, can be used for mapping folders to external source.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> Update(string projectId, string name, string collectionId = null,
            string collectionKey = null, string newName = null, string sourceId = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if (projectId == null || name == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("folder.update",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    name,
                    new_name = newName,
                    source_id = sourceId
                });
        }

        /// <summary>
        /// Adds container-level permission to specified User API client. Requires Backend API key with Admin permission role.
        /// <remarks>The collection_id/collection_key parameter means that one can use either one of them - collection_id or collection_key.</remarks>
        /// </summary>
        /// <param name="apiClientId">User API client id.</param>
        /// <param name="permission">User API client's permission to add.</param>
        /// <param name="projectId">Project containing specified container.</param>
        /// <param name="folderName">Folder name defining folder that permission will be added to.</param>
        /// <param name="collectionId">Collection containing specified container.</param>
        /// <param name="collectionKey">Collection containing specified container.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> Authorize(string apiClientId, Permissions permission, string projectId, string folderName,
            string collectionId = null, string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if(apiClientId == null || projectId == null || folderName == null)
                throw new ArgumentNullException();

            string permissionString = PermissionsParser.GetString(permission);

            return _syncanoClient.GetAsync("folder.authorize",
                new
                {
                    api_client_id = apiClientId,
                    permission = permissionString,
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    folder_name = folderName
                });
        }

        /// <summary>
        /// Removes container-level permission from specified User API client. Requires Backend API key with Admin permission role.
        /// <remarks>The collection_id/collection_key parameter means that one can use either one of them - collection_id or collection_key.</remarks>
        /// </summary>
        /// <param name="apiClientId">User API client id.</param>
        /// <param name="permission">User API client's permission to remove.</param>
        /// <param name="projectId">Project containing specified container.</param>
        /// <param name="folderName">Folder name defining folder that permission will be removed from.</param>
        /// <param name="collectionId">Collection containing specified container.</param>
        /// <param name="collectionKey">Collection containing specified container.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> Deauthorize(string apiClientId, Permissions permission, string projectId, string folderName,
            string collectionId = null, string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if (apiClientId == null || projectId == null || folderName == null)
                throw new ArgumentNullException();

            string permissionString = PermissionsParser.GetString(permission);

            return _syncanoClient.GetAsync("folder.deauthorize",
                new
                {
                    api_client_id = apiClientId,
                    permission = permissionString,
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    folder_name = folderName
                });
        }

        /// <summary>
        /// Permanently delete specified folder and all associated data.
        /// <remarks>The collection_id/collection_key parameter means that one can use either one of them - collection_id or collection_key.</remarks>
        /// </summary>
        /// <param name="projectId">Project id.</param>
        /// <param name="name">Folder name to delete.</param>
        /// <param name="collectionId">Collection id defining collection where folder exists.</param>
        /// <param name="collectionKey">Collection key defining collection where folder exists.</param>
        /// <returns>Boolen value indicating success of method.</returns>
        public Task<bool> Delete(string projectId, string name, string collectionId = null, string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if (projectId == null || name == null)
                throw new ArgumentNullException();

            return _syncanoClient.GetAsync("folder.delete", new { project_id = projectId, collection_id = collectionId, collection_key = collectionKey, name });
        }
    }
}
