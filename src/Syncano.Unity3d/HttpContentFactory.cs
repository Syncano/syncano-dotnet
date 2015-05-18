using System;
using System.Linq;
using System.Text;
using Syncano4.Shared;

namespace Syncano4.Unity3d
{
    class HttpContentFactory
    {
        public HttpContent Create( IRequestContent requestContent)
        {
            if (requestContent is FormRequestContent)
                return Create((FormRequestContent) requestContent);
            if(requestContent is JsonRequestContent)
                return Create((JsonRequestContent)requestContent);

            throw new NotSupportedException("Cannot create HttpContent for: " + requestContent);

        }

        protected HttpContent Create(JsonRequestContent jsonRequestContent)
        {
            HttpContent httpContent = new HttpContent();
            httpContent.ContentType = "application/json";
            httpContent.Content = jsonRequestContent.Json;
            return httpContent;
        }

        protected HttpContent Create( FormRequestContent formRequestContent)
        {
            HttpContent httpContent = new HttpContent();
            httpContent.ContentType = "application/x-www-form-urlencoded";

            StringBuilder sb = new StringBuilder();

            bool firstParam = true;
            foreach (var parameter in formRequestContent.Parameters.Where(p => p.Value != null))
            {
                if (!firstParam)
                    sb.Append("&");
                else
                {
                    firstParam = false;
                }

                sb.AppendFormat("{0}={1}", parameter.Key,Uri.EscapeDataString(parameter.Value.ToString()));
            }
            httpContent.Content = sb.ToString();
            return httpContent;
        }
    }

    public class HttpContent
    {
        public string ContentType { get; set; }

        public string Content { get; set; }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(Content);
        }
    }
}