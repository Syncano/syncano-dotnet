using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    public class DataRelationNotification : BaseNotification
    {
        [JsonProperty("target")]
        public RelationTarget Target { get; set; }
    }
}