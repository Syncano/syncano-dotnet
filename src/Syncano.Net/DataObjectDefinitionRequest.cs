using System.Collections.Generic;
using Syncano.Net.Data;

namespace Syncano.Net
{
    /// <summary>
    /// Request defining data object.
    /// </summary>
    public class DataObjectDefinitionRequest
    {
        public DataObjectDefinitionRequest()
        {
            Folder = "Default";
            State = DataObjectState.Pending;
        }

        /// <summary>
        /// Project id.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// Collection id defining collection for which data will be created.
        /// </summary>
        public string CollectionId { get; set; }

        /// <summary>
        /// Collection key defining collection for which data will be created.
        /// </summary>
        public string CollectionKey { get; set; }

        /// <summary>
        /// Used for uniquely identifying message. Has to be unique within collection. Useful for updating.
        /// </summary>
        public string DataKey { get; set; }

        /// <summary>
        /// User name of user to associate Data Object with. If not set, internal user 'syncano' is used.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Source URL associated with message.
        /// </summary>
        public string SourceUrl { get; set; }

        /// <summary>
        /// Title of message.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Text data associated with message.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Link associated with message.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Image data associated with message.
        /// </summary>
        public string ImageBase64 { get; set; }

        /// <summary>
        /// Image source URL. Used in combination with image parameter.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Additional integer data that is filterable in data.get().
        /// </summary>
        public long? DataOne { get; set; }

        /// <summary>
        /// Additional integer data that is filterable in data.get().
        /// </summary>
        public long? DataTwo { get; set; }

        /// <summary>
        /// Additional integer data that is filterable in data.get().
        /// </summary>
        public long? DataThree { get; set; }

        /// <summary>
        /// Folder name that data will be put in. Default value: 'Default'.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// State of data to be initially set. Accepted values: Pending, Moderated, Rejected. Default value: Pending.
        /// </summary>
        public DataObjectState State { get; set; }

        /// <summary>
        /// If specified, creates one parent-child relation with specified parent id. You can add more by using data.add_parent().
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Any number of additional parameters will be saved into an Additional dictionary.
        /// </summary>
        public Dictionary<string, string> Additional { get; set; } 

    }
}
