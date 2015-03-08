using System;
using System.Collections.Generic;
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
            _baseUrl = string.Format("https://syncanotest1-env.elasticbeanstalk.com/v1/");
            _client = new HttpClient();
        }

        private string CreateGetUri(string methodName, object query = null)
        {
            var sb = new StringBuilder(CreateBaseUri(methodName));
            sb.Append(CreateParametersString(query));
            return sb.ToString();
        }

        private string CreateBaseUri(string methodName)
        {
            var sb = new StringBuilder(_baseUrl);
            sb.Append(methodName);
            sb.Append("/?api_key=");
            sb.Append(_apiKey);
            return sb.ToString();
        }

        public async Task<string> GetAsync(string methodName, IDictionary<string, object> parameters )
        {
             var response = await _client.GetAsync(CreateGetUri(methodName, parameters));
             var content = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(content);
            return json.SelectToken("result").Value<string>();
        }


        public async Task<IList<T>> GetAsync<T>(string methodName, IDictionary<string, object> parameters )
        {
            var response = await _client.GetAsync(CreateGetUri(methodName, parameters));
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SyncanoResponse<T>>(content).Objects;
            
        }

   

        public async Task<T> PostAsync<T>(string endpoint, IDictionary<string, object> parameters )
        {

            var postContent = new FormUrlEncodedContent(parameters.ToDictionary(p => p.Key, p => Uri.EscapeDataString(p.Value.ToString())));
            var response = await _client.PostAsync(CreateGetUri(endpoint, null), postContent);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new SyncanoException(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

       

        private string CreateParametersString(object query)
        {
            var sb = new StringBuilder();

            if (query != null)
            {
                foreach (var each in query.GetType().GetRuntimeProperties())
                {
                    if (each.GetValue(query, null) != null)
                    {
                        if (each.GetValue(query, null).GetType().IsArray)
                        {
                            var array = (Array) each.GetValue(query, null);

                            foreach (var item in array)
                            {
                                sb.AppendFormat("&{0}={1}", each.Name, Uri.EscapeDataString(item.ToString()));
                            }
                        }
                        else
                        {
                            sb.AppendFormat("&{0}={1}", each.Name, Uri.EscapeDataString(each.GetValue(query, null).ToString()));
                        }
                    }
                }
            }

            return sb.ToString();
        }
    }
}