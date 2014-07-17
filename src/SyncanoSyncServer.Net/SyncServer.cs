using System.Threading.Tasks;
using Syncano.Net;
using Syncano.Net.Api;

namespace SyncanoSyncServer.Net
{
    public class SyncServer
    {
        private readonly string _instanceName;
        private readonly string _api;
        private SyncServerClient _syncClient;

        public SyncServer(string instanceName, string api)
        {
            _instanceName = instanceName;
            _api = api;
        }


        public async Task<LoginResult> Start()
        {
            _syncClient = new SyncServerClient();
            await _syncClient.Connect();
            return await _syncClient.Login(_api, _instanceName);
        }

        public ProjectSyncanoClient Projects { get { return new ProjectSyncanoClient(_syncClient); } }
    }
}