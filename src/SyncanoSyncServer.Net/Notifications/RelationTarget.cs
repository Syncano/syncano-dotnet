using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    /// <summary>
    /// Class representiing target in relation notifications.
    /// <remarks> If both keys are specified, notification is meant to represent a deletion of just one parent-child relation. If one of the keys is missing, it represents mass delete of relations. Example: "target": {"parent_id": "123456", "child_id": "321654"} (delete this specific relation), "target": {"parent_id": "123456"} (delete all relations where "123456" is a parent - in other words delete all children of "123456"), "target": {"child_id": "123456"} (delete all parents of "123456").</remarks>
    /// </summary>
    public class RelationTarget
    {
        /// <summary>
        /// Id of parent DataObject.
        /// </summary>
        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        /// <summary>
        /// Id of child DataObject.
        /// </summary>
        [JsonProperty("child_id")]
        public string ChildId { get; set; }
    }
}
