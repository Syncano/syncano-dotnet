using System;
using System.Threading.Tasks;
using Syncano.Net;
using Syncano.Net.Api;
using SyncanoSyncServer.Net.Notifications;

namespace SyncanoSyncServer.Net
{
    public class SyncServer
    {
        protected readonly string _instanceName;
        protected readonly string _api;
        protected SyncServerClient _syncClient;

        
        public IObservable<NewDataNotification> NewDataObservable { get
        {
            return _syncClient.NewDataNotificationObservable;
        } }
        public IObservable<DeleteDataNotification> DeleteDataObservable { get
        {
            return _syncClient.DeleteDataNotificationObservable;
        } }

        public IObservable<ChangeDataNotification> ChangeDataObservable { get
        {
            return _syncClient.ChangeDataNotificationObservable;
        } } 

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

        public void Stop()
        {
            _syncClient.Disconnect();
        }

        public ProjectSyncanoClient Projects { get { return new ProjectSyncanoClient(_syncClient); } }

        public FolderSyncanoClient Folders { get { return new FolderSyncanoClient(_syncClient); } }

        public CollectionSyncanoClient Collections { get { return new CollectionSyncanoClient(_syncClient); } }

        public DataObjectSyncanoClient DataObjects { get { return new DataObjectSyncanoClient(_syncClient); } }

        public AdministratorSyncanoClient Administrators { get { return new AdministratorSyncanoClient(_syncClient); } }

        public ApiKeySyncanoClient ApiKeys { get { return new ApiKeySyncanoClient(_syncClient); } }

        public UserSyncanoClient Users { get { return new UserSyncanoClient(_syncClient); } }

        public RealTimeSyncSyncanoClient RealTimeSync { get { return new RealTimeSyncSyncanoClient(_syncClient);} }
    }
}