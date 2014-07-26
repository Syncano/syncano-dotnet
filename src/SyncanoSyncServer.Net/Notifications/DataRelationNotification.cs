using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    /// <summary>
    /// These notifications are sent whenever there is a change in parent-child connections between any two (or more) Data Objects. Only two types of changes are possible: new and delete. There is only one common additional field for these types.
    /// </summary>
    public class DataRelationNotification : BaseNotification
    {
        /// <summary>
        /// Target information.
        /// </summary>
        [JsonProperty("target")]
        public RelationTarget Target { get; set; }
    }
}