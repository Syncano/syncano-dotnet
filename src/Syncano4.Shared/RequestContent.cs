using System;
using System.Collections.Generic;
using System.Text;

namespace Syncano4.Shared
{
    public interface IRequestContent
    {
    }

    class FormRequestContent : IRequestContent
    {
        public FormRequestContent(IDictionary<string, object> parameters)
        {
            Parameters = parameters;
        }

        public IDictionary<string, object> Parameters { get; }
    }

    class JsonRequestContent : IRequestContent
    {
        public string Json { get; set; }
    }


}
