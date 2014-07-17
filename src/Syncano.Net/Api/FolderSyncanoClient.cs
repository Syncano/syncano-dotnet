using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Syncano.Net.Data;
using Syncano.Net.Http;

namespace Syncano.Net.Api
{
    public class FolderSyncanoClient
    {
        private readonly SyncanoHttpClient _httpClient;

        public FolderSyncanoClient(SyncanoHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<Folder> New(string projectId, string name, string collectionId = null,
            string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if(projectId == null || name == null)
                throw new ArgumentNullException();

            return _httpClient.PostAsync<Folder>("folder.new", new { project_id = projectId, collection_id = collectionId, collection_key = collectionKey, name },
                "folder");
        }

        public async Task<List<Folder>> Get(string projectId, string collectionId = null,
            string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if(projectId == null)
                throw new ArgumentNullException();

            return await _httpClient.GetAsync<List<Folder>>("folder.get", new { project_id = projectId, collection_id = collectionId, collection_key = collectionKey }, "folder");
        }

        public Task<Folder> GetOne(string projectId, string folderName, string collectionId = null,
            string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if(projectId == null || folderName == null)
                throw new ArgumentNullException();

            return _httpClient.GetAsync<Folder>("folder.get_one", new { project_id = projectId, collection_id = collectionId, collection_key = collectionKey, folder_name = folderName }, "folder");
        }

        public Task<bool> Update(string projectId, string name, string collectionId = null,
            string collectionKey = null, string newName = null, string sourceId = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if (projectId == null || name == null)
                throw new ArgumentNullException();

            return _httpClient.PostAsync("folder.update",
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

        public Task<bool> Authorize(string apiClientId, Permissions permission, string projectId, string folderName,
            string collectionId = null, string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if(apiClientId == null || projectId == null || folderName == null)
                throw new ArgumentNullException();

            string permissionString = PermissionsParser.GetString(permission);

            return _httpClient.GetAsync("folder.authorize",
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

        public Task<bool> Deauthorize(string apiClientId, Permissions permission, string projectId, string folderName,
            string collectionId = null, string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if (apiClientId == null || projectId == null || folderName == null)
                throw new ArgumentNullException();

            string permissionString = PermissionsParser.GetString(permission);

            return _httpClient.GetAsync("folder.deauthorize",
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

        public Task<bool> Delete(string projectId, string name, string collectionId = null, string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            if (projectId == null || name == null)
                throw new ArgumentNullException();

            return _httpClient.GetAsync("folder.delete", new { project_id = projectId, collection_id = collectionId, collection_key = collectionKey, name });
        }
    }
}
