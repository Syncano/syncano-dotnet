using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Syncano.Net.Data
{
    /// <summary>
    /// Representing DataObject.
    /// <remarks>Data Objects always live in the top level of a parent-child relationship hierarchy (the parent of all parents). It is not possible to change that. However, one can also make the Data Object a child of other Data Objects. E.g. if Object #1 resides in the folder “Pictures,” and Object #2 resides in the folder “Authors,” Object #2 can be set as a child of Object #1. It will still reside in it’s original folder, but there will be a parent-child connection between these Data Objects. It is possible to define all kinds of data relationships: one-to-many, many-to-one, one-to-one, many-to-many. When using data.new() or data.update() you can only pass one parent_id parameter, which is sufficient for one-to-one or one-to-many relationships. To add more parent-child connections (called Data Object Relations in Syncano) use data.add_parent() or data.add_child(). Remove of these connections with data.remove_parent() and data.remove_child(). Data Objects can have multiple parents, however they cannot be removed from the structure of its folder/collection.</remarks>
    /// </summary>
    public class DataObject
    {
        /// <summary>
        /// Id of data for future reference.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Date and time of creation.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Date and time of last update.
        /// </summary>
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Folder name.
        /// </summary>
        [JsonProperty("folder")]
        public string Folder { get; set; }

        /// <summary>
        /// Possible values: Pending, Moderated, Rejected.
        /// </summary>
        [JsonProperty("state")]
        [JsonConverter(typeof(DataObjectStateEnumConverter))]
        public DataObjectState State { get; set; }

        /// <summary>
        /// Owner of DataObject.
        /// </summary>
        [JsonProperty("user")]
        public User User { get; set; }

        /// <summary>
        /// Used to uniquely define data object
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// DataObject title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// DataObject text content.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// DataObject link.
        /// </summary>
        [JsonProperty("link")]
        public string Link { get; set; }

        /// <summary>
        /// Additional data for querying.
        /// </summary>
        [JsonProperty("data1")]
        public long? DataOne { get; set; }

        /// <summary>
        /// Additional data for querying.
        /// </summary>
        [JsonProperty("data2")]
        public long? DataTwo { get; set; }

        /// <summary>
        /// Additional data for querying.
        /// </summary>
        [JsonProperty("data3")]
        public long? DataThree { get; set; }

        /// <summary>
        /// URL associated with data object's source.
        /// </summary>
        [JsonProperty("source_url")]
        public string SourceUrl { get; set; }

        /// <summary>
        /// DataObject image content.
        /// </summary>
        [JsonProperty("image")]
        public Image Image { get; set; }

        /// <summary>
        /// Consisting of arbitrary string data defined as key - value (string) pairs.
        /// </summary>
        [JsonProperty("additional")]
        public Dictionary<string, string> Additional { get; set; }

        /// <summary>
        /// Number of associated children
        /// </summary>
        [JsonProperty("children_count")]
        public int ChildrenCount { get; set; }

        /// <summary>
        /// Consisting of child Data Object arrays (same structure, recursively), present if include_children is true and there are children defined.
        /// </summary>
        [JsonProperty("children")]
        public List<DataObject> Children { get; set; }


    }
}
