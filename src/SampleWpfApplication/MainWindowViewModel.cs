using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using Syncano.Net.DataRequests;
using SyncanoSyncServer.Net;
using System;
using DataObject = Syncano.Net.Data.DataObject;

namespace SampleWpfApplication
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Syncano.Net.Syncano _syncano;
        private SyncServer _syncServer;

        public MainWindowViewModel()
        {
            DataObjectsHttp = new ObservableCollection<DataObject>();
            DataObjectsSync = new ObservableCollection<DataObject>();

            InstanceName = "icy-brook-267066";
            ApiKey = "f020f3a62b2ea236100a732adcf60cb98683e2e5";
            ProjectId = "1625";
            CollectionId = "6490";
            FolderName = "Default";
            SelectedHttp = -1;
            SelectedSync = -1;
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
            _syncServer.NewDataObservable.SubscribeOnDispatcher().Subscribe(n =>
            {
                App.Current.Dispatcher.Invoke(() => DataObjectsSync.Add(n.Data));
            });

            _syncServer.DeleteDataObservable.SubscribeOnDispatcher().Subscribe(n =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    RefreshDataObjectsSync();
                });
            });

            //Create Http connection object
            _syncano = new Syncano.Net.Syncano(this.InstanceName, this.ApiKey);

            //Load existing objects
            RefreshDataObjectsHttp();
            RefreshDataObjectsSync();
        }
        private async void Cleanup()
        {
            if(_syncServer == null)
                return;

            if ((await _syncServer.RealTimeSync.GetSubscriptions()).Any(s => s.Type == "Project" && s.Id == this.ProjectId) == true)
                await _syncServer.RealTimeSync.UnsubscribeProject(ProjectId);
        }

        private async void RefreshDataObjectsHttp()
        {
            var dataObjects =
                await
                    _syncano.DataObjects.Get(new DataObjectRichQueryRequest()
                    {
                        ProjectId = ProjectId,
                        CollectionId = CollectionId,
                        Folder = FolderName
                    });

            DataObjectsHttp.Clear();
            foreach (var dataObject in dataObjects)
                DataObjectsHttp.Add(dataObject);
        }
        private async void RefreshDataObjectsSync()
        {
            var dataObjects =
                await
                    _syncServer.DataObjects.Get(new DataObjectRichQueryRequest()
                    {
                        ProjectId = ProjectId,
                        CollectionId = CollectionId,
                        Folder = FolderName
                    });

            DataObjectsSync.Clear();
            foreach (var dataObject in dataObjects)
                DataObjectsSync.Add(dataObject);
        }

        private async void AddDataObjectHttp()
        {
            var dialog = new AddDataObjectWindow();
            if (dialog.ShowDialog() != true)
                return;
            var request = CreateDataDefinitaionRequest(dialog.Title.Text, dialog.Text.Text, dialog.Link.Text, dialog.Image.Text, dialog.Additionals);

            await _syncano.DataObjects.New(request);
            RefreshDataObjectsHttp();
        }
        private async void AddDataObjectSync()
        {
            var dialog = new AddDataObjectWindow();
            if (dialog.ShowDialog() != true)
                return;
            var request = CreateDataDefinitaionRequest(dialog.Title.Text, dialog.Text.Text, dialog.Link.Text, dialog.Image.Text, dialog.Additionals);

            await _syncServer.DataObjects.New(request);
        }
        private DataObjectDefinitionRequest CreateDataDefinitaionRequest(string title, string text, string link,
            string imagePath, ObservableCollection<AdditionalItem> additionalItems)
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
            return request;
        }

        public async void DeleteObjectHttp()
        {
            var dataObject = DataObjectsHttp[SelectedHttp];
            DataObjectsHttp.Remove(dataObject);
            SelectedHttp = -1;

            var request = new DataObjectSimpleQueryRequest
            {
                ProjectId = ProjectId,
                CollectionId = CollectionId,
                Folder = FolderName,
                DataId = dataObject.Id
            };

            await _syncano.DataObjects.Delete(request);
        }
        public async void DeleteObjectSync()
        {
            var dataObject = DataObjectsSync[SelectedSync];
            DataObjectsHttp.Remove(dataObject);
            SelectedSync = -1;

            var request = new DataObjectSimpleQueryRequest
            {
                ProjectId = ProjectId,
                CollectionId = CollectionId,
                Folder = FolderName,
                DataId = dataObject.Id
            };

            await _syncServer.DataObjects.Delete(request);
        }

        public ObservableCollection<DataObject> DataObjectsHttp { get; set; }
        public ObservableCollection<DataObject> DataObjectsSync { get; set; } 


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
        public string FolderName
        {
            get { return _folderName; }
            set
            {
                _folderName = value;
                OnPropertyChanged("FolderName");
            }
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                OnPropertyChanged("IsConnected");
            }
        }

        private int _selectedHttp;
        public int SelectedHttp
        {
            get { return _selectedHttp; }
            set
            {
                _selectedHttp = value;
                OnPropertyChanged("SelectedHttp");
            }
        }

        private int _selectedSync;
        public int SelectedSync
        {
            get { return _selectedSync; }
            set
            {
                _selectedSync = value;
                OnPropertyChanged("SelectedSync");
            }
        }

        public ICommand ConnectCommand
        {
            get { return new RelayCommand(Connect); }
        }
        public ICommand RefreshHttpCommand
        {
            get { return new RelayCommand(RefreshDataObjectsHttp);}
        }
        public ICommand AddHttpCommand
        {
            get { return new RelayCommand(AddDataObjectHttp);}
        }
        public ICommand AddSynCommand
        {
            get { return new RelayCommand(AddDataObjectSync);}
        }
        public ICommand DeleteHttpCommand
        {
            get { return new RelayCommand(DeleteObjectHttp);}
        }
        public ICommand DeleteSyncCommand
        {
            get { return new RelayCommand(DeleteObjectSync);}
        }
        public ICommand WindowClosing
        {
            get { return new RelayCommand(Cleanup); }
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