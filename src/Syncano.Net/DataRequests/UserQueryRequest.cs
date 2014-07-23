using System.Collections.Generic;
using Syncano.Net.Data;

namespace Syncano.Net.DataRequests
{
    /// <summary>
    /// Request for querying users.
    /// </summary>
    public class UserQueryRequest
    {
        /// <summary>
        /// Creates UserQueryRequest object.
        /// </summary>
        public UserQueryRequest()
        {
            State = DataObjectState.All;
        }

        /// <summary>
        /// Project id.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// Collection id defining collection.
        /// </summary>
        public string CollectionId { get; set; }

        /// <summary>
        /// Collection key defining collection.
        /// </summary>
        public string CollectionKey { get; set; }

        /// <summary>
        /// If specified returns only users whose Data Objects are in specified state. Possible values: Pending, Moderated, All. Default value: All.
        /// </summary>
        public DataObjectState State { get; set; }

        /// <summary>
        /// If specified returns only users whose Data Objects are in specified folder.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// If specified returns only users whose Data Objects are in specified folders.
        /// </summary>
        public List<string> Folders { get; set; }

        /// <summary>
        /// If specified filtering by related Data Object's content. Possible values: TEXT - only return users that sent data with text, IMAGE - only return users that sent data with an image.
        /// </summary>
        public DataObjectContentFilter? Filter { get; set; }


    }
}
