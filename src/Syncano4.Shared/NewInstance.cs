using System.Collections.Generic;
using Newtonsoft.Json;

namespace Syncano4.Shared
{
    public class NewInstance 
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
       
    }
}