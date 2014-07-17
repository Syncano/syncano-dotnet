using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Syncano.Net.Data
{
    public class DataObject
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("folder")]
        public string Folder { get; set; }

        [JsonProperty("state")]
        [JsonConverter(typeof(DataObjectStateEnumConverter))]
        public DataObjectState State { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("data1")]
        public long? DataOne { get; set; }

        [JsonProperty("data2")]
        public long? DataTwo { get; set; }

        [JsonProperty("data3")]
        public long? DataThree { get; set; }

        [JsonProperty("source_url")]
        public string SourceUrl { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }

        [JsonProperty("additional")]
        public Dictionary<string, string> Additional { get; set; }

        [JsonProperty("children_count")]
        public int ChildrenCount { get; set; }

        [JsonProperty("children")]
        public List<DataObject> Children { get; set; }


    }
}
