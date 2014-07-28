using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
            _syncano = new Syncano.Net.Syncano(InstanceName, ApiKey);
            _syncServer = new SyncServer(InstanceName, ApiKey);

            RefreshDataObjects();
            InitSyncServer();
        }

        public async void InitSyncServer()
        {
            await _syncServer.Start();

            try
            {
                await _syncServer.RealTimeSync.SubscribeProject(ProjectId);
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

            //Subscribe to data relations notifications
            _syncServer.DataRelationObservable.Subscribe(n =>
            {
                App.Current.Dispatcher.Invoke((Action)(() => Notifications.Add(n)));
            });
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
            get
            {
                return _instanceName;
            }
            set
            {
                _instanceName = value;
                OnPropertyChanged("InstanceName");
            }
        }

        private string _apiKey;

        public string ApiKey
        {
            get
            {
                return _apiKey;
            }
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
            get
            {
                return _collectionid;
            }
            set
            {
                _collectionid = value;
                OnPropertyChanged("CollectionId");
            }
        }

        private string _folderName;

        public string FolderName
        {
            get { return _folderName; }
            set
            {
                _folderName = value;
                OnPropertyChanged("FolderName");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged = null;
        virtual protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
