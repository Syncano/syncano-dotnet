using System.Collections.Generic;
using Newtonsoft.Json;

namespace SyncanoSyncServer.Net.Notifications
{
    public class DeleteDataNotification : BaseNotification
    {
        [JsonProperty("target")]
        public Target Target { get; set; }
    }
}
