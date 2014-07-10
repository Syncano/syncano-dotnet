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

        public async Task<List<Folder>> GetFoldersByCollectionId(string projectId, string collectionId)
        {
            return await GetAsync("folder.get", new {project_id = projectId, collection_id = collectionId}, "folder",
                        t => t.ToObject<List<Folder>>());
        }

        public async Task<List<Folder>> GetFoldersByCollectionKey(string projectId, string collectionKey)
        {
            return await GetAsync("folder.get", new { project_id = projectId, collection_key = collectionKey }, "folder",
                        t => t.ToObject<List<Folder>>());
        }
    }
}