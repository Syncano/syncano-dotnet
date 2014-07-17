using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Syncano.Net.Http
{
    public class SyncanoHttpClient : ISyncanoClient
    {
        private readonly string _instanceName;
        private readonly string _apiKey;
        private HttpClient _client;
        private string _baseUrl;

        public SyncanoHttpClient(string instanceName, string apiKey)
        {
            _instanceName = instanceName;
            _apiKey = apiKey;
            _baseUrl = string.Format("https://{0}.syncano.com/api/", _instanceName);
            _client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip, UseCookies = true });
        }

        private string CreateGetUri(string methodName, object query = null)
        {
            var sb = new StringBuilder(CreateBaseUri(methodName));
            sb.Append(CreateParametersString(methodName, query));
            return sb.ToString();
        }

        private string CreateBaseUri(string methodName)
        {
            var sb = new StringBuilder(_baseUrl);
            sb.Append(methodName);
            sb.Append("?api_key=");
            sb.Append(_apiKey);
            return sb.ToString();
        }

        private string CreateParametersString(string methodName, object query)
        {
            var sb = new StringBuilder();
            
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

        public async Task<T> GetAsync<T>(string methodName, object parameters, string contentToken, Func<JToken, T> getResult)
        {
            var response = await _client.GetStringAsync(CreateGetUri(methodName, parameters));
            var json = CheckResponseStatus(response);

            return getResult(json.SelectToken(contentToken));
        }

        public async Task<T> PostAsync<T>(string methodName, object parameters, string contentToken, Func<JToken, T> getResult)
        {
            var content = CreatePostContent(parameters);
            var response = await _client.PostAsync(CreateBaseUri(methodName), content);
            var json = CheckResponseStatus(await response.Content.ReadAsStringAsync());

            return getResult(json.SelectToken(contentToken));
        }

        private HttpContent CreatePostContent(object query)
        {
            var content = new MultipartFormDataContent();

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
                                content.Add( new StringContent(item.ToString()),each.Name);
                            }
                        }
                        else if (each.GetValue(query).GetType().IsConstructedGenericType && each.GetValue(query).GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
                        {
                            var dictionary = (Dictionary<string,string>)each.GetValue(query);
                            foreach (var item in dictionary)
                            {
                                content.Add(new StringContent(item.Value), item.Key);
                            }
                        }
                        else
                        {
                            content.Add(new StringContent(each.GetValue(query).ToString()), each.Name);
                        }
                    }

                }
            }



            return content;
        }

        public async Task<bool> GetAsync(string methodName, object parameters)
        {
            var response = await _client.GetStringAsync(CreateGetUri(methodName, parameters));
            var json = CheckResponseStatus(response);

            return true;
        }

        public async Task<bool> PostAsync(string methodName, object parameters)
        {
            var content = CreatePostContent(parameters);
            var response = await _client.PostAsync(CreateBaseUri(methodName), content);
            var json = CheckResponseStatus(await response.Content.ReadAsStringAsync());

            return true;
        }

        public Task<string> StartSession()
        {
            return GetAsync("apikey.start_session", "session_id", t => t.Value<string>());
        }

        

    }
}