using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Syncano4.Shared;

namespace Syncano4.Net
{
    public class SyncanoHttpClient : ISyncanoHttpClient
    {
        private readonly string _apiKey;
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        /// <summary>
        /// Creates SyncanoHttpClientObject.
        /// </summary>
        /// <param name="instanceName">Name of Syncano instance.</param>
        /// <param name="apiKey">Api key for Syncano instance (backend or user).</param>
        public SyncanoHttpClient(string apiKey)
        {
            _apiKey = apiKey;
            _baseUrl = string.Format("https://syncanotest1-env.elasticbeanstalk.com");
            _client = new HttpClient();
        }

        private string CreateGetUri(string methodName, IDictionary<string, object> query = null)
        {
            var sb = new StringBuilder(CreateBaseUri(methodName));
            sb.Append(CreateParametersString(query));
            return sb.ToString();
        }

        private string CreateBaseUri(string link)
        {
            var sb = new StringBuilder(_baseUrl);
            sb.Append(link);

            if (link.Contains("?"))
                sb.Append("&");
            else
                sb.Append("?");

            sb.Append("api_key=");
            sb.Append(_apiKey);
            return sb.ToString();
        }

        public async Task<string> GetAsync(string methodName, IDictionary<string, object> parameters)
        {
            var response = await _client.GetAsync(CreateGetUri(methodName, parameters));
            var content = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(content);
            return json.SelectToken("result").Value<string>();
        }


        public async Task<SyncanoResponse<T>> ListAsync<T>(string methodName, IDictionary<string, object> parameters)
        {
            var response = await _client.GetAsync(CreateGetUri(methodName, parameters));
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SyncanoResponse<T>>(content);
        }

        public async Task<T> GetAsync<T>(string methodName)
        {
            var response = await _client.GetAsync(CreateGetUri(methodName, null));
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }


        public async Task<T> PostAsync<T>(string endpoint, IDictionary<string, object> parameters)
        {
            var postContent =
                new FormUrlEncodedContent(parameters.Where(p => p.Value != null)
                    .ToDictionary(p => p.Key, p => p.Value is DateTime ? ((DateTime) p.Value).ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ") : p.Value.ToString()));
            var response = await _client.PostAsync(CreateGetUri(endpoint, null), postContent);
            if (new[] {HttpStatusCode.Created,HttpStatusCode.OK}.Contains(response.StatusCode) == false)
            {
                throw new SyncanoException(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        private string CreateParametersString(IDictionary<string, object> query)
        {
            var sb = new StringBuilder();

            if (query != null)
            {
                foreach (var each in query)
                {
                    if (each.Value != null)
                    {
                        if (each.Value.GetType().IsArray)
                        {
                            var array = (Array) each.Value;

                            foreach (var item in array)
                            {
                                sb.AppendFormat("&{0}={1}", each.Key, Uri.EscapeDataString(item.ToString()));
                            }
                        }
                        else
                        {
                            sb.AppendFormat("&{0}={1}", each.Key, Uri.EscapeDataString(each.Value.ToString()));
                        }
                    }
                }
            }

            return sb.ToString();
        }
    }
}