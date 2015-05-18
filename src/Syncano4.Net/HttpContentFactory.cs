using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using Syncano4.Shared;

namespace Syncano4.Net
{
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