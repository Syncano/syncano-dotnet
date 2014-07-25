﻿using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    public class DataRelationNotification : BaseNotification
    {
        [JsonProperty("child_id")]
        public long ChildId { get; set; }

        [JsonProperty("parent_id")]
        public long ParentId { get; set; }
    }
}