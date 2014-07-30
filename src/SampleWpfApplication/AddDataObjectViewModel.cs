using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Microsoft.Win32;
using Syncano.Net.DataRequests;

namespace SampleWpfApplication
{
    public class AddDataObjectViewModel
    {
        private AddDataObjectWindow _window;

        public ObservableCollection<AdditionalItem> Additionals { get; set; }

        public AddDataObjectViewModel()
        {

            Additionals = new ObservableCollection<AdditionalItem>();

            _window = new AddDataObjectWindow();
            _window.DataContext = this;
        }

        public void ShowDialog()
        {
            _window.ShowDialog();
        }

        private bool _wasSuccess;
        public bool WasSuccess
        {
            get { return _wasSuccess; }
            set
            {
                _wasSuccess = value;
                OnPropertyChanged("WasSuccess");
            }
        }

        public DataObjectDefinitionRequest Request
        {
            get { return CreateDataDefinitaionRequest(_window.Title.Text, _window.Text.Text, _window.Link.Text, _window.ImagePath.Text, Additionals); }
        }

        private void Add()
        {
            WasSuccess = true;
            _window.Close();
        }

        private void Cancel()
        {
            WasSuccess = false;
            _window.Close();
        }

        private void Load()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            dialog.Title = "Load image";

            if (dialog.ShowDialog() == true)
                _window.ImagePath.Text = dialog.FileName;
        }

        public ICommand AddCommand
        {
            get { return new RelayCommand(Add);}
        }

        public ICommand CancelCommand
        {
            get { return new RelayCommand(Cancel);}
        }

        public ICommand LoadCommand
        {
            get { return new RelayCommand(Load);}
        }

        private DataObjectDefinitionRequest CreateDataDefinitaionRequest(string title, string text, string link,
            string imagePath, ObservableCollection<AdditionalItem> additionalItems)
        {
            var request = new DataObjectDefinitionRequest();
            
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

        public event PropertyChangedEventHandler PropertyChanged = null;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
