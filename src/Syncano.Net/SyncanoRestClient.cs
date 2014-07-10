using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Syncano.Net
{
    public class SyncanoRestClient
    {
        private readonly string _instanceName;
        private readonly string _apiKey;
        private HttpClient _client;
        private string _baseUrl;

        public SyncanoRestClient(string instanceName, string apiKey)
        {
            _instanceName = instanceName;
            _apiKey = apiKey;
            _baseUrl = string.Format("https://{0}.syncano.com/api/", _instanceName);
            _client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip, UseCookies = true });
        }

        private string CreateGetUri(string methodName, object query = null)
        {
            var sb = new StringBuilder(_baseUrl);
            sb.Append(methodName);
            sb.Append("?api_key=");
            sb.Append(_apiKey);

            if (query != null)
            {
                foreach (var each in Type.GetTypeFromHandle(query.GetType().TypeHandle).GetRuntimeProperties())
                {
                    if (each.GetValue(query) != null)
                        sb.AppendFormat("&{0}={1}", each.Name, Uri.EscapeDataString(each.GetValue(query).ToString()));
                }
            }

            return sb.ToString();
        }

        private JObject CheckResponseStatus(string response)
        {
            var json = JObject.Parse(response);
            var result = json.SelectToken("result").Value<string>();
            if (result == null)
                throw new SyncanoException("Unexpected response: " + response);

            if (result == "NOK")
                throw new SyncanoException("Error: " + json.SelectToken("error").Value<string>());

            return json;
        }

        private async Task<T> GetAsync<T>(string methodName, string contentToken, Func<JToken, T> getResult)
        {
            return await GetAsync<T>(methodName, null, contentToken, getResult);
        }

        private async Task<T> GetAsync<T>(string methodName, object query, string contentToken, Func<JToken, T> getResult)
        {
            var response = await _client.GetStringAsync(CreateGetUri(methodName, query));
            var json = CheckResponseStatus(response);

            return getResult(json.SelectToken(contentToken));
        }

        private async Task<bool> GetAsync(string methodName, object query)
        {
            var response = await _client.GetStringAsync(CreateGetUri(methodName, query));
            var json = CheckResponseStatus(response);

            return true;
        }

        public Task<string> StartSession()
        {
            return GetAsync("apikey.start_session", "session_id", t => t.Value<string>());
        }

        public Task<Project> NewProject(string name, string description = null)
        {
            return GetAsync("project.new", new {name = name, description = description}, "project",
                t => t.ToObject<Project>());
        }

        public async Task<List<Project>> GetProjects()
        {
            return await GetAsync("project.get", "project", t => t.ToObject<List<Project>>());
        }

        public Task<Project> GetProject(string projectId)
        {
            return GetAsync("project.get_one", new { project_id = projectId }, "project", t => t.ToObject<Project>());
        }

        public Task<Project> UpdateProject(string projectId, string name = null, string description = null)
        {
            return GetAsync("project.update", new {project_id = projectId, name = name, description = description},
                "project", t => t.ToObject<Project>());
        }

        public Task<bool> DeleteProject(string projectId)
        {
            return GetAsync("project.delete", new {project_id = projectId});
        }

        public Task<Folder> NewFolder(string projectId, string name, string collectionId = null,
            string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return GetAsync("folder.new", new { project_id = projectId, collection_id = collectionId, collection_key = collectionKey, name = name },
                "folder", t => t.ToObject<Folder>());
        }

        public async Task<List<Folder>> GetFolders(string projectId, string collectionId = null,
            string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return await GetAsync("folder.get", new { project_id = projectId, collection_id = collectionId, collection_key = collectionKey }, "folder",
                        t => t.ToObject<List<Folder>>());
        }

        public Task<Folder> GetFolder(string projectId, string folderName, string collectionId = null,
            string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return GetAsync("folder.get_one", new { project_id = projectId, collection_id = collectionId, collection_key = collectionKey, folder_name = folderName }, "folder",
                t => t.ToObject<Folder>());
        }

        public Task<bool> UpdateFolder(string projectId, string name, string collectionId = null,
            string collectionKey = null, string newName = null, string sourceId = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return GetAsync("folder.update",
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

        public Task<bool> AuthorizeFolder(string apiClientId, Permissions permission, string projectId, string folderName,
            string collectionId = null, string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            string permissionString = PermissionsParser.GetString(permission);

            return GetAsync("folder.authorize",
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

        public Task<bool> DeauthorizeFolder(string apiClientId, Permissions permission, string projectId, string folderName,
            string collectionId = null, string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            string permissionString = PermissionsParser.GetString(permission);

            return GetAsync("folder.deauthorize",
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

        public Task<bool> DeleteFolder(string projectId, string name, string collectionId = null, string collectionKey = null)
        {
            if (collectionId == null && collectionKey == null)
                throw new ArgumentNullException();

            return GetAsync("folder.delete", new { project_id = projectId, collection_id = collectionId, collection_key = collectionKey, name = name });
        }


    }
}