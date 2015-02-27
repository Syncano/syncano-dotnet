using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Syncano4.Shared;

namespace Syncano4.Net
{
    public class SyncanoHttpClient
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

        public async Task<string> GetAsync(string methodName, object parameters)
        {
            var response = await _client.GetStringAsync(CreateGetUri(methodName, parameters));

            return response;
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

    public class SyncanoInstances
    {
        public SyncanoInstances(string apiKey)
        {
            _syncanoHttpClient = new SyncanoHttpClient(apiKey);
        }

        private SyncanoHttpClient _syncanoHttpClient;

        public async Task<List<Instance>> Get()
        {
            return new List<Instance>() { new Instance() { Name = await _syncanoHttpClient.GetAsync("instances", null) } };
        }
    }

  
}