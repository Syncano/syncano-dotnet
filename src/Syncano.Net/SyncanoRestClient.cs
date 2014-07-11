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
                    {
                        if (each.GetValue(query).GetType().IsArray)
                        {
                            var array = (Array)each.GetValue(query);

                            foreach (var item in array)
                            {
                                sb.AppendFormat("&{0}={1}", each.Name, Uri.EscapeDataString(item.ToString()));
                            }
                        }
                        else
                        {
                            sb.AppendFormat("&{0}={1}", each.Name, Uri.EscapeDataString(each.GetValue(query).ToString()));
                        }
                    }

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

        public async Task<T> GetAsync<T>(string methodName, string contentToken, Func<JToken, T> getResult)
        {
            return await GetAsync<T>(methodName, null, contentToken, getResult);
        }

        public async Task<T> GetAsync<T>(string methodName, object query, string contentToken, Func<JToken, T> getResult)
        {
            var response = await _client.GetStringAsync(CreateGetUri(methodName, query));
            var json = CheckResponseStatus(response);

            return getResult(json.SelectToken(contentToken));
        }

        public async Task<bool> GetAsync(string methodName, object query)
        {
            var response = await _client.GetStringAsync(CreateGetUri(methodName, query));
            var json = CheckResponseStatus(response);

            return true;
        }

        public Task<string> StartSession()
        {
            return GetAsync("apikey.start_session", "session_id", t => t.Value<string>());
        }

        

    }
}