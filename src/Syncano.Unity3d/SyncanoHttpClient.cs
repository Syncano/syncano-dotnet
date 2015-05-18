using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Syncano4.Shared;
using Syncano4.Shared.Serialization;

namespace Syncano4.Unity3d
{
    public class SyncanoHttpClient : ISyncanoHttpClient
    {
        private readonly string _apiKey;
        private readonly WebClient _client;
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
            _contentFactory = new HttpContentFactory();
            _client = new WebClient();
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        private string CreateBaseUri(string methodName, IDictionary<string, object> parameters)
        {
            var sb = new StringBuilder(_baseUrl);
            sb.Append(methodName);

            if (methodName.Contains("?"))
                sb.Append("&");
            else
                sb.Append("?");

            sb.Append("api_key=");
            sb.Append(_apiKey);

            if (parameters != null)
            {
                foreach (var each in parameters)
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

        private HttpWebRequest CreateRequest(string methodName, string httpMethod,
            IDictionary<string, object> parameters)
        {
            var request = (HttpWebRequest) WebRequest.Create(CreateBaseUri(methodName, parameters));
            request.Method = httpMethod;
            return request;
        }

        public SyncanoResponse<T> List<T>(string methodName, IDictionary<string, object> parameters)
        {
            var content = Get(methodName, parameters);

            return SyncanoJsonConverter.DeserializeObject<SyncanoResponse<T>>(content);
        }


        public T Get<T>(string link)
        {
            try
            {
                var content = Get(link, null);
                return SyncanoJsonConverter.DeserializeObject<T>(content);
            }
            catch (WebException e)
            {
                var httpResponse = e.Response as HttpWebResponse;
                if (httpResponse != null && httpResponse.StatusCode == HttpStatusCode.NotFound)
                    return default(T);

                throw;
            }
        }

        public void Delete(string link)
        {
            var request = CreateRequest(link, "DELETE", null);
            var response = (HttpWebResponse) request.GetResponse();

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new SyncanoException("Failed to delete." + GetResponseContent(response));
            }
        }

        public T Patch<T>(string endpoint, IRequestContent requestContent)
        {
            var request = CreateRequest(endpoint, "PATCH", null);
            return SendRequest<T>(requestContent, request);
        }


        public T Post<T>(string endpoint, IRequestContent requestContent)
        {
            var request = CreateRequest(endpoint, WebRequestMethods.Http.Post, null);
            return SendRequest<T>(requestContent, request);
        }

        private T SendRequest<T>(IRequestContent requestContent, HttpWebRequest request)
        {
            var content = _contentFactory.Create(requestContent);
            request.ContentType = content.ContentType;
            var bytes = content.GetBytes();
            request.ContentLength = bytes.Length;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
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

                return SyncanoJsonConverter.DeserializeObject<T>(responseString);
            }
            catch (WebException e)
            {
                var message = GetResponseContent(e.Response);
                throw new SyncanoException(message);
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }

        private static string GetResponseContent(WebResponse response)
        {
            string message = null;
            using (var s = response.GetResponseStream())
            {
                using (var r = new StreamReader(s))
                {
                    message = r.ReadToEnd();
                }
            }
            return message;
        }
    }
}