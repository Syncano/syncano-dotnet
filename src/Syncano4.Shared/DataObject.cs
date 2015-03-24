using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;



namespace Syncano4.Shared
{
    public class DataObject:IArgs
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("revision")]
        public int Revision { get; private set; }
        
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; private set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; private set; }
        
        [JsonProperty("links")]
        public Dictionary<string, string> Links { get; private set; }


        public IDictionary<string, object> ToDictionary()
        {
            var jsonString = JsonConvert.SerializeObject(this);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

            dictionary.Remove("id");
            dictionary.Remove("revision");
            dictionary.Remove("created_at");

            dictionary.Remove("updated_at");
            dictionary.Remove("links");

            return dictionary;
        }
    }
}