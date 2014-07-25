using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    public class RelationTarget
    {
        [JsonProperty("parent_id")]
        public long ParentId { get; set; }

        [JsonProperty("child_id")]
        public long ChildId { get; set; }
    }
}
