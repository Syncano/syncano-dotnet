using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;



namespace Syncano4.Shared
{
    public class Instance
    {
        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("description")]
        public string Description { get; set; }


        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("links")]
        public Dictionary<string, string> Links { get; set; }



    }
}