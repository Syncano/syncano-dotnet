using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Syncano4.Shared;

namespace Syncano4.Unity3d
{
    public class SyncanoHttpClient : ISyncanoHttpClient
    {
        private readonly string _apiKey;
        private readonly WebClient _client;
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
            _client = new WebClient();
        }
        
        private string CreateBaseUri(string methodName, IDictionary<string, object> parameters)
        {
            var sb = new StringBuilder(_baseUrl);
            sb.Append(methodName);
            sb.Append("?api_key=");
            sb.Append(_apiKey);

            if (parameters != null)
            {
                foreach (var each in parameters)
                {
                    if (each.Value != null)
                    {
                        if (each.Value.GetType().IsArray)
                        {
                            var array = (Array)each.Value;

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

        public string Get(string methodName, IDictionary<string, object> parameters)
        {
            var request = CreateRequest(methodName, WebRequestMethods.Http.Get, parameters);
            using (var response = (HttpWebResponse) request.GetResponse())
            {
                using (var s = response.GetResponseStream())
                {
                    using (var r = new StreamReader(s))
                    {
                        return r.ReadToEnd();
                    }
                }
            }
        }

        private HttpWebRequest CreateRequest(string methodName, string httpMethod, IDictionary<string, object> parameters)
        {
            var request = (HttpWebRequest)WebRequest.Create(CreateBaseUri(methodName, parameters));
            request.Method = httpMethod;
            return request;
        }

        public IList<T> List<T>(string methodName, IDictionary<string, object> parameters)
        {
            var content = Get(methodName, parameters);

            return JsonConvert.DeserializeObject<SyncanoResponse<T>>(content).Objects;
        }

        public T Get<T>(string link)
        {
            var content = Get(link, null);

            return JsonConvert.DeserializeObject<T>(content);
        }
        

        public T Post<T>(string endpoint, IDictionary<string, object> parameters)
        {
            var request = CreateRequest(endpoint, WebRequestMethods.Http.Post, null);
            request.ContentType = "application/x-www-form-urlencoded";

            StringBuilder sb = new StringBuilder();

            bool firstParam = true;
            foreach (var parameter in parameters.Where(p => p.Value != null))
            {
                if (!firstParam)
                    sb.Append("&");
                else
                {
                    firstParam = false;
                }

                sb.AppendFormat("{0}={1}", parameter.Key, parameter.Value is DateTime ? ((DateTime)parameter.Value).ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ") : Uri.EscapeDataString(parameter.Value.ToString())/*.Replace("%20","+")*/);
            }
            var postBytes = Encoding.UTF8.GetBytes(sb.ToString());
            request.ContentLength = postBytes.Length;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(postBytes, 0, postBytes.Length);
                requestStream.Flush();
            }
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse) request.GetResponse();
                string responseString = null;
                using (var s = response.GetResponseStream())
                {
                    using (var r = new StreamReader(s))
                    {
                        responseString = r.ReadToEnd();
                    }
                }

                return JsonConvert.DeserializeObject<T>(responseString);
            }
            catch (WebException e)
            {
                string message = null;
                using (var s = e.Response.GetResponseStream())
                {
                    using (var r = new StreamReader(s))
                    {
                        message = r.ReadToEnd();
                    }
                }
                throw new SyncanoException(message);
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }


        private string CreateParametersString(object query)
        {
            var sb = new StringBuilder();

            if (query != null)
            {
                foreach (var each in query.GetType().GetProperties())
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