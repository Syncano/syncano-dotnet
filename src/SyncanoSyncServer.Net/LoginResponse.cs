using Newtonsoft.Json;
using SyncanoSyncServer.Net.Notifications;

namespace SyncanoSyncServer.Net
{
    /// <summary>
    /// Response to login request.
    /// </summary>
    public class LoginResponse : BaseNotification
    {
        /// <summary>
        /// Unique id of connection.
        /// </summary>
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        /// <summary>
        /// Result of login operation.
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }


        /// <summary>
        /// description of an error
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }

        
        
    }
}