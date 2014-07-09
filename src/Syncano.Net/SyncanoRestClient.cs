using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userApiKey"></param>
        /// <returns></returns>
        public async Task<Response> StartSession()
        {
            var response = await _client.GetStringAsync(CreateGetUri("apikey.start_session"));

            return JsonConvert.DeserializeObject<Response>(response);
        }

        private string CreateGetUri(string methodName, ExpandoObject query = null)
        {
            var sb = new StringBuilder(_baseUrl);
            sb.Append(methodName);
            sb.Append("?api_key=");
            sb.Append(_apiKey);

            if (query != null)
            {
                foreach (var each in query)
                {
                    sb.AppendFormat("&{0}={1}", each.Key, Uri.EscapeDataString(each.Value.ToString()));
                }
            }

            return sb.ToString();
        }

        public async Task<List<Project>> GetProjects()
        {
            var response = await _client.GetStringAsync(CreateGetUri("project.get"));

            var projects = JObject.Parse(response).SelectToken("project");
            
            return projects.ToObject<List<Project>>();

        }
    }


    public class Project
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

    }

    public class Response
    {
        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("session_id")]
        public string SessionId { get; set; }
    }
}