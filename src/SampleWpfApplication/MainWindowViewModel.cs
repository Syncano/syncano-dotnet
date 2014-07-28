using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Syncano.Net.Data;
using Syncano.Net.DataRequests;
using SyncanoSyncServer.Net;
using SyncanoSyncServer.Net.Notifications;
using System;

namespace SampleWpfApplication
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private const string InstanceName = "icy-brook-267066";
        private const string BackendKey = "f020f3a62b2ea236100a732adcf60cb98683e2e5";
        private const string DefaultProjectId = "1625";
        private const string CollectionId = "6490";
        private const string FolderName = "Default";

        private Syncano.Net.Syncano _syncano;
        private SyncServer _syncServer;

        public MainWindowViewModel()
        {
            DataObjects = new ObservableCollection<DataObject>();
            Notifications = new ObservableCollection<BaseNotification>();

            //Connect to Syncano
            _syncano = new Syncano.Net.Syncano(InstanceName, BackendKey);
            _syncServer = new SyncServer(InstanceName, BackendKey);

            RefreshDataObjects();
            InitSyncServer();
        }

        public async void InitSyncServer()
        {
            await _syncServer.Start();

            try
            {
                await _syncServer.RealTimeSync.SubscribeProject(DefaultProjectId);
            }
            catch (Exception) { }

            //Subscribe to new data notifications
            _syncServer.NewDataObservable.Subscribe(n =>
            {
                App.Current.Dispatcher.Invoke((Action) (() => Notifications.Add(n)));
            });

            //Subscribe to delete data notifications
            _syncServer.DeleteDataObservable.Subscribe(n =>
            {
                App.Current.Dispatcher.Invoke((Action) (() => Notifications.Add(n)));
            });

            //Subscribe to generic notifications
            _syncServer.GenericNotificationObservable.Subscribe(n =>
            {
                App.Current.Dispatcher.Invoke((Action)(() => Notifications.Add(n)));
            });
        }

        public async void Cleanup()
        {
            await _syncServer.RealTimeSync.UnsubscribeProject(DefaultProjectId);
        }

        public async void RefreshDataObjects()
        {
            var dataObjects =
                await
                    _syncano.DataObjects.Get(new DataObjectRichQueryRequest()
                    {
                        ProjectId = DefaultProjectId,
                        CollectionId = CollectionId,
                        Folder = FolderName
                    });

            DataObjects.Clear();
            foreach (var dataObject in dataObjects)
                DataObjects.Add(dataObject);
        }

        public async void DeleteDataObject(DataObject dataObject)
        {
            DataObjects.Remove(dataObject);

            await _syncano.DataObjects.Delete(new DataObjectSimpleQueryRequest()
            {
                ProjectId = DefaultProjectId,
                CollectionId = CollectionId,
                Folder = FolderName,
                DataId = dataObject.Id
            });
        }

        public async void AddDataObject(string title, string text)
        {
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = DefaultProjectId;
            request.CollectionId = CollectionId;
            request.Folder = FolderName;
            request.Title = title;
            request.Text = text;

            await _syncano.DataObjects.New(request);
            RefreshDataObjects();
        }
        

        public ObservableCollection<DataObject> DataObjects { get; set; }

        public ObservableCollection<BaseNotification> Notifications { get; set; } 


        public event PropertyChangedEventHandler PropertyChanged = null;
        virtual protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
