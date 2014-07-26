using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    /// <summary>
    /// Notification about removed DataObjects.
    /// </summary>
    public class DeleteDataNotification : BaseNotification
    {
        /// <summary>
        /// Target information.
        /// </summary>
        [JsonProperty("target")]
        public DataTarget Target { get; set; }
    }
}
