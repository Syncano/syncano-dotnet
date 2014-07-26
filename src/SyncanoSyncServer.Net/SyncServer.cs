using System;
using System.Threading.Tasks;
using Syncano.Net.Api;
using SyncanoSyncServer.Net.Notifications;
using SyncanoSyncServer.Net.RealTimeSyncApi;

namespace SyncanoSyncServer.Net
{
    /// <summary>
    /// Class makes possible to connect to Syncano Sync Server over Tcp. Like in class Syncano, in Rest Api module, you can manage your Syncano Instance using it, but also it provides api for subscribing and listening for real time notifications that inform you, what is happening with your Syncano Instance right now.
    /// <remarks>The Sync Server is meant to facilitate notification/subscription processing. To control what you subscribe to or what you want to send, refer to the notification and subscription API documentation. Once you are subscribed to and connected to the Sync Server, you will start receiving real-time notifications.</remarks>
    /// </summary>
    public class SyncServer
    {
        private readonly string _instanceName;
        private readonly string _api;
        private SyncServerClient _syncClient;

        /// <summary>
        /// Observable providing notifications about new DataObjects in Syncano Instance.
        /// </summary>
        public IObservable<NewDataNotification> NewDataObservable { get
        {
            return _syncClient.NewDataNotificationObservable;
        } }

        /// <summary>
        /// Observable providing notifications about deleted DataObjects in Syncano Instance.
        /// </summary>
        public IObservable<DeleteDataNotification> DeleteDataObservable { get
        {
            return _syncClient.DeleteDataNotificationObservable;
        } }

        /// <summary>
        /// Observable providing notifications about modified DataObjects in Syncano Instance.
        /// </summary>
        public IObservable<ChangeDataNotification> ChangeDataObservable { get
        {
            return _syncClient.ChangeDataNotificationObservable;
        } }

        /// <summary>
        /// Observable providing notifications about DataObjects relations in Syncano Instance (ex. dataObject has new child or all children has been removed from dataObject).
        /// </summary>
        public IObservable<DataRelationNotification> DataRelationObservable { get
        {
            return _syncClient.DataRelationNotificationObservable;
        } }

        /// <summary>
        /// Observable providing some generic notifications about Syncano Instance (ex. new server logged in or custom notification send by other user).
        /// </summary>
        public IObservable<GenericNotification> GenericNotificationObservable { get
        {
            return _syncClient.GenericNotificationObservable;
        } } 

        /// <summary>
        /// Creates SyncServer object.
        /// </summary>
        /// <param name="instanceName">Name of Syncano Instance.</param>
        /// <param name="api">Api Key of Syncano Instance.</param>
        public SyncServer(string instanceName, string api)
        {
            _instanceName = instanceName;
            _api = api;
        }

        /// <summary>
        /// Starts SyncServer and connects it to Syncano Sync Server.
        /// </summary>
        /// <returns>LoginResult object with information about operation status.</returns>
        public async Task<LoginResult> Start()
        {
            _syncClient = new SyncServerClient();
            await _syncClient.Connect();
            return await _syncClient.Login(_api, _instanceName);
        }

        /// <summary>
        /// Disconnects SyncServer.
        /// </summary>
        public void Stop()
        {
            _syncClient.Disconnect();
        }

        /// <summary>
        /// Provides access to Project api.
        /// </summary>
        public ProjectSyncanoClient Projects { get { return new ProjectSyncanoClient(_syncClient); } }

        /// <summary>
        /// Provides access to Folder api.
        /// </summary>
        public FolderSyncanoClient Folders { get { return new FolderSyncanoClient(_syncClient); } }

        /// <summary>
        /// Provides access to Collection api.
        /// </summary>
        public CollectionSyncanoClient Collections { get { return new CollectionSyncanoClient(_syncClient); } }

        /// <summary>
        /// Provides access to DataObject api.
        /// </summary>
        public DataObjectSyncanoClient DataObjects { get { return new DataObjectSyncanoClient(_syncClient); } }

        /// <summary>
        /// Provides access to Administrator api.
        /// </summary>
        public AdministratorSyncanoClient Administrators { get { return new AdministratorSyncanoClient(_syncClient); } }

        /// <summary>
        /// Provides access to ApiKey api.
        /// </summary>
        public ApiKeySyncanoClient ApiKeys { get { return new ApiKeySyncanoClient(_syncClient); } }

        /// <summary>
        /// Provides access to User api.
        /// </summary>
        public UserSyncanoClient Users { get { return new UserSyncanoClient(_syncClient); } }

        /// <summary>
        /// Provides access to Real Time Sync api. It is used to manage notifications, subscriptions and connections.
        /// </summary>
        public RealTimeSyncSyncanoClient RealTimeSync { get { return new RealTimeSyncSyncanoClient(_syncClient);} }
    }
}