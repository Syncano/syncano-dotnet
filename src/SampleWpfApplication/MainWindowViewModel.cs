using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Syncano.Net.Data;
using Syncano.Net.DataRequests;
using SyncanoSyncServer.Net;
using SyncanoSyncServer.Net.Notifications;
using System;

namespace SampleWpfApplication
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Syncano.Net.Syncano _syncano;
        private SyncServer _syncServer;

        public MainWindowViewModel()
        {
            DataObjects = new ObservableCollection<DataObject>();
            Notifications = new ObservableCollection<BaseNotification>();

            InstanceName = "icy-brook-267066";
            ApiKey = "f020f3a62b2ea236100a732adcf60cb98683e2e5";
            ProjectId = "1625";
            CollectionId = "6490";
            FolderName = "Default";

            //Connect to Syncano
        }

      
        public async void Cleanup()
        {
            await _syncServer.RealTimeSync.UnsubscribeProject(ProjectId);
        }

        public async void RefreshDataObjects()
        {
            var dataObjects =
                await
                    _syncano.DataObjects.Get(new DataObjectRichQueryRequest()
                    {
                        ProjectId = ProjectId,
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
                ProjectId = ProjectId,
                CollectionId = CollectionId,
                Folder = FolderName,
                DataId = dataObject.Id
            });
        }

        public async void AddDataObject(string title, string text, string link, string imagePath, ObservableCollection<AdditionalItem> additionalItems)
        {
            var request = new DataObjectDefinitionRequest();
            request.ProjectId = ProjectId;
            request.CollectionId = CollectionId;
            request.Folder = FolderName;
            request.Title = title;
            request.Text = text;
            request.Link = link;

            if (imagePath != "")
                using (var f = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var ms = new MemoryStream())
                    {
                        f.CopyTo(ms);
                        byte[] imageBytes = ms.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        request.ImageBase64 = base64String;
                    }
                }

            request.Additional = new Dictionary<string, string>();
            foreach (var item in additionalItems)
                request.Additional.Add(item.Key, item.Value);

            await _syncano.DataObjects.New(request);
            RefreshDataObjects();
        }


        public ObservableCollection<DataObject> DataObjects { get; set; }

        public ObservableCollection<BaseNotification> Notifications { get; set; }

        private string _instanceName;

        public string InstanceName
        {
            get { return _instanceName; }
            set
            {
                _instanceName = value;
                OnPropertyChanged("InstanceName");
            }
        }

        private string _apiKey;

        public string ApiKey
        {
            get { return _apiKey; }
            set
            {
                _apiKey = value;
                OnPropertyChanged("ApiKey");
            }
        }

        private string _projectId;

        public string ProjectId
        {
            get { return _projectId; }
            set
            {
                _projectId = value;
                OnPropertyChanged("ProjectId");
            }
        }

        private string _collectionid;

        public string CollectionId
        {
            get { return _collectionid; }
            set
            {
                _collectionid = value;
                OnPropertyChanged("CollectionId");
            }
        }

        private string _folderName;
        private bool _isConnected;

        public string FolderName
        {
            get { return _folderName; }
            set
            {
                _folderName = value;
                OnPropertyChanged("FolderName");
            }
        }

        public ICommand ConnectCommand
        {
            get { return new RelayCommand(Connect); }
        }

        private async void Connect()
        {
            //Login
            _syncServer = new SyncServer(this.InstanceName, this.ApiKey);
            var login = await _syncServer.Start();
            this.IsConnected = login.WasSuccessful;

            //Add subscriptions.
            if ((await _syncServer.RealTimeSync.GetSubscriptions()).Any(s => s.Type == "Project" && s.Id == this.ProjectId) == false)
                await _syncServer.RealTimeSync.SubscribeProject(ProjectId);

            //React on subscriptions using reactive extensions

            //Subscribe to new data notifications
            _syncServer.NewDataObservable.SubscribeOnDispatcher().Subscribe( Notifications.Add);

            //Subscribe to delete data notifications
            _syncServer.DeleteDataObservable.SubscribeOnDispatcher().Subscribe(Notifications.Add);

            //Subscribe to generic notifications
            _syncServer.GenericNotificationObservable.SubscribeOnDispatcher().Subscribe(Notifications.Add);

            //Subscribe to data relations notifications
            _syncServer.DataRelationObservable.SubscribeOnDispatcher().Subscribe(Notifications.Add);


            _syncano = new Syncano.Net.Syncano(this.InstanceName, this.ApiKey);
  }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                OnPropertyChanged("IsConnected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = null;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}