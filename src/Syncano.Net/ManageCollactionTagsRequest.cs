using System.Collections.Generic;
namespace Syncano.Net
{
    /// <summary>
    /// Request object for methods adding and deleting collection tags.
    /// </summary>
    public class ManageCollactionTagsRequest
    {
        /// <summary>
        /// Project id.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// Event ID.
        /// </summary>
        public string CollectionId { get; set; }

        /// <summary>
        /// Event codeword.
        /// </summary>
        public string CollectionKey { get; set; }

        /// <summary>
        /// Tag to be added or deleted.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Tags to be added or deleted.
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
