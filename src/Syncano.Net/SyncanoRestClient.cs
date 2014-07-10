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
            _client = new HttpClient(new HttpClientHandler() {AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip, UseCookies = true});
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
                    sb.AppendFormat("&{0}={1}", each.Name, Uri.EscapeDataString(each.GetValue(query).ToString()));
                }
            }

            return sb.ToString();
        }

        private async Task<T> GetAsync<T>(string methodName, string contentToken, Func<JToken, T> getResult)
        {
            return await GetAsync<T>(methodName, null, contentToken, getResult);
        }

        private async Task<T> GetAsync<T>(string methodName, object query, string contentToken, Func<JToken, T> getResult)
        {
            var response = await _client.GetStringAsync(CreateGetUri(methodName, query));

            var json = JObject.Parse(response);
            var result = json.SelectToken("result").Value<string>();
            if (result == null)
                throw new SyncanoException("Unexpected response: " + response);

            if (result == "NOK")
                throw new SyncanoException("Error: " + json.SelectToken("error").Value<string>());


            return getResult(json.SelectToken(contentToken));
        }

        private async Task GetAsync(string methodName, object query)
        {
            var response = await _client.GetStringAsync(CreateGetUri(methodName, query));

            var json = JObject.Parse(response);
            var result = json.SelectToken("result").Value<string>();
            if (result == null)
                throw new SyncanoException("Unexpected response: " + response);

            if (result == "NOK")
                throw new SyncanoException("Error: " + json.SelectToken("error").Value<string>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userApiKey"></param>
        /// <returns></returns>
        public Task<string> StartSession()
        {
            return GetAsync("apikey.start_session", "session_id", t => t.Value<string>());
        }

        public async Task<List<Project>> GetProjects()
        {
            return await GetAsync("project.get", "project", t => t.ToObject<List<Project>>());
        }

        public Task<Project> GetProject(string projectId)
        {
            return GetAsync("project.get_one", new { project_id = projectId  }, "project", t => t.ToObject<Project>());
        }

        private Task<Folder> NewFolderByCollectionId(string projectId, string collectionId, string name)
        {
            return GetAsync("folder.new", new {project_id = projectId, collection_id = collectionId, name = name},
                "folder", t => t.ToObject<Folder>());
        }

        private Task<Folder> NewFolderByCollectionKey(string projectId, string collectionKey, string name)
        {
            return GetAsync("folder.new", new { project_id = projectId, collection_key = collectionKey, name = name },
                "folder", t => t.ToObject<Folder>());
        }

        public Task<Folder> NewFolder(string projectId, string name, string collectionId = null,
            string collectionKey = null)
        {
            if (collectionId != null)
                return NewFolderByCollectionId(projectId, collectionId, name);

            if (collectionKey != null)
                return NewFolderByCollectionKey(projectId, collectionKey, name);

            throw new ArgumentNullException();
        } 

        private Task<List<Folder>> GetFoldersByCollectionId(string projectId, string collectionId)
        {
            return GetAsync("folder.get", new {project_id = projectId, collection_id = collectionId}, "folder",
                        t => t.ToObject<List<Folder>>());
        }

        private async Task<List<Folder>> GetFoldersByCollectionKey(string projectId, string collectionKey)
        {
            return await GetAsync("folder.get", new { project_id = projectId, collection_key = collectionKey }, "folder",
                        t => t.ToObject<List<Folder>>());
        }

        public async Task<List<Folder>> GetFolders(string projectId, string collectionId = null,
            string collectionKey = null)
        {
            if (collectionId != null)
                return await GetFoldersByCollectionId(projectId, collectionId);

            if (collectionKey != null)
                return await GetFoldersByCollectionKey(projectId, collectionKey);

            throw  new ArgumentNullException();
        }

        private Task<Folder> GetFolderByCollectionId(string projectId, string collectionId, string folderName)
        {
            return GetAsync("folder.get_one", new {project_id = projectId, collection_id = collectionId, folder_name = folderName}, "folder",
                t => t.ToObject<Folder>());
        }

        private Task<Folder> GetFolderByCollectionKey(string projectId, string collectionKey, string folderName)
        {
            return GetAsync("folder.get_one", new { project_id = projectId, collection_key = collectionKey, folder_name = folderName }, "folder",
                t => t.ToObject<Folder>());
        }

        public Task<Folder> GetFolder(string projectId, string folderName, string collectionId = null,
            string collectionKey = null)
        {
            if (collectionId != null)
                return GetFolderByCollectionId(projectId, collectionId, folderName);

            if (collectionKey != null)
                return GetFolderByCollectionKey(projectId, collectionKey, folderName);

            throw new ArgumentNullException();
        }

        private Task DeleteFolderByCollectionId(string projectId, string name, string collectionId)
        {
            return GetAsync("folder.delete", new {project_id = projectId, collection_id = collectionId, name = name});
        }

        private Task DeleteFolderByCollectionKey(string projectId, string name, string collectionKey)
        {
            return GetAsync("folder.delete", new { project_id = projectId, collection_key = collectionKey, name = name });
        }

        public Task DeleteFolder(string projectId, string name, string collectionId = null, string collectionKey = null)
        {
            if (collectionId != null)
                return DeleteFolderByCollectionId(projectId, name, collectionId);

            if (collectionKey != null)
                return DeleteFolderByCollectionKey(projectId, name, collectionKey);

            throw new ArgumentNullException();
        }

        
    }
}