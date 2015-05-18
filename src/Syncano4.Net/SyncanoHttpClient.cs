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
using Syncano4.Shared.Serialization;

namespace Syncano4.Net
{
    public class SyncanoHttpClient : ISyncanoHttpClient
    {
        private readonly string _apiKey;
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://api.syncano.io";
        private HttpContentFactory _contentFactory;

        /// <summary>
        /// Creates SyncanoHttpClientObject.
        /// </summary>
        /// <param name="instanceName">Name of Syncano instance.</param>
        /// <param name="apiKey">Api key for Syncano instance (backend or user).</param>
        public SyncanoHttpClient(string apiKey)
        {
            _apiKey = apiKey;
            _client = new HttpClient();
            _contentFactory = new HttpContentFactory();
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

            return SyncanoJsonConverter.DeserializeObject<SyncanoResponse<T>>(content);
        }

        public async Task<T> GetAsync<T>(string methodName)
        {
            var response = await _client.GetAsync(CreateGetUri(methodName, null));

            if (!response.IsSuccessStatusCode)
                return default(T);

            var content = await response.Content.ReadAsStringAsync();

            return SyncanoJsonConverter.DeserializeObject<T>(content);
        }

        public async Task DeleteAsync(string link)
        {
            var response = await _client.DeleteAsync(CreateGetUri(link));
            if (response.StatusCode != HttpStatusCode.NoContent)
                throw new SyncanoException(await response.Content.ReadAsStringAsync());
        }


        public async Task<T> PostAsync<T>(string endpoint, IRequestContent requestContent)
        {
            HttpContent postContent = _contentFactory.Create(requestContent);
            var response = await _client.PostAsync(CreateGetUri(endpoint, null), postContent);
            if (new[] {HttpStatusCode.Created, HttpStatusCode.OK}.Contains(response.StatusCode) == false)
            {
                throw new SyncanoException(await response.Content.ReadAsStringAsync());
            }
            return SyncanoJsonConverter.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        public async Task<T> PatchAsync<T>(string endpoint, IRequestContent requestContent)
        {
            HttpContent postContent = _contentFactory.Create(requestContent);
            var msg = new HttpRequestMessage(new HttpMethod("PATCH"), CreateGetUri(endpoint, null));
            msg.Content = postContent;
            var response = await _client.SendAsync(msg);
            if (new[] {HttpStatusCode.Created, HttpStatusCode.OK}.Contains(response.StatusCode) == false)
            {
                throw new SyncanoException(await response.Content.ReadAsStringAsync());
            }
            return SyncanoJsonConverter.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
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

     class HttpContentFactory
    {
        public HttpContent Create(IRequestContent requestContent)
        {
            if (requestContent is FormRequestContent)
                return Create((FormRequestContent) requestContent);
            if(requestContent is JsonRequestContent)
                return Create((JsonRequestContent)requestContent);

            throw new NotSupportedException("Cannot create HttpContent for: " + requestContent);

        }

        protected HttpContent Create(JsonRequestContent jsonRequestContent)
        {
           return new StringContent(jsonRequestContent.Json, Encoding.UTF8, "application/json");
        }

        protected HttpContent Create(FormRequestContent formRequestContent)
        {
           return new FormUrlEncodedContent(formRequestContent.Parameters.Where(p => p.Value != null)
                     .ToDictionary(p => p.Key, p => p.Value.ToString()));
        }
    }
}