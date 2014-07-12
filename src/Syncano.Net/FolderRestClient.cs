using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncano.Net
{
    public class FolderRestClient
    {
        private readonly SyncanoRestClient _restClient;

        public FolderRestClient(SyncanoRestClient restClient)
        {
            _restClient = restClient;
        }

        public Task<Folder> New(string projectId, string name, string collectionId = null,
            string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("folder.new", new { project_id = projectId, collection_id = collectionId, collection_key = collectionKey, name = name },
                "folder", t => t.ToObject<Folder>());
        }

        public async Task<List<Folder>> Get(string projectId, string collectionId = null,
            string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return await _restClient.GetAsync("folder.get", new { project_id = projectId, collection_id = collectionId, collection_key = collectionKey }, "folder",
                        t => t.ToObject<List<Folder>>());
        }

        public Task<Folder> GetOne(string projectId, string folderName, string collectionId = null,
            string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("folder.get_one", new { project_id = projectId, collection_id = collectionId, collection_key = collectionKey, folder_name = folderName }, "folder",
                t => t.ToObject<Folder>());
        }

        public Task<bool> Update(string projectId, string name, string collectionId = null,
            string collectionKey = null, string newName = null, string sourceId = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return _restClient.GetAsync("folder.update",
                new
                {
                    project_id = projectId,
                    collection_id = collectionId,
                    collection_key = collectionKey,
                    name = name,
                    new_name = newName,
                    source_id = sourceId
                });
        }

        public Task<bool> Authorize(string apiClientId, Permissions permission, string projectId, string folderName,
            string collectionId = null, string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            string permissionString = PermissionsParser.GetString(permission);

            return _restClient.GetAsync("folder.authorize",
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

            string permissionString = PermissionsParser.GetString(permission);

            return _restClient.GetAsync("folder.deauthorize",
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

            return _restClient.GetAsync("folder.delete", new { project_id = projectId, collection_id = collectionId, collection_key = collectionKey, name = name });
        }
    }
}
