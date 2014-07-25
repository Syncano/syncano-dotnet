using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    public class RelationTarget
    {
        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("child_id")]
        public string ChildId { get; set; }
    }
}
