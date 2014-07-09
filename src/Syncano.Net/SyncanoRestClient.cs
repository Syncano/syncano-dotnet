using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Syncano.Net
{
    public class SyncanoRestClient
    {
        private HttpClient _client;

        public SyncanoRestClient()
        {
            _client = new HttpClient(new HttpClientHandler() {AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip, UseCookies = true});
        }

        public async Task<Response> StartSession(string instanceName, string userApiKey)
        {
            var response = await _client.GetStringAsync(string.Format("https://{0}.syncano.com/api/apikey.start_session?api_key={1}", instanceName, userApiKey));

            return JsonConvert.DeserializeObject<Response>(response);
        }
    }


    public class Response
    {
        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("session_id")]
        public string SessionId { get; set; }
    }
}