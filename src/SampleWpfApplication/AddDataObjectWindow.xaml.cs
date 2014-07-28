using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace SampleWpfApplication
{
    /// <summary>
    /// Interaction logic for AddDataObjectWindow.xaml
    /// </summary>
    public partial class AddDataObjectWindow : Window
    {
        public ObservableCollection<AdditionalItem> Additionals { get; set; }

        public AddDataObjectWindow()
        {
            InitializeComponent();

            Additionals = new ObservableCollection<AdditionalItem>();
            DataGrid.DataContext = Additionals;
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void LoadImageClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            dialog.Title = "Load image";

            if (dialog.ShowDialog() == true)
                Image.Text = dialog.FileName;
        }
    }
}
