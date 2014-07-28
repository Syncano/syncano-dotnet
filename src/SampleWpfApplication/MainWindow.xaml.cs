using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Syncano.Net.Data;
using Syncano.Net.DataRequests;


namespace SampleWpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            //Set data context
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;
        }

        private void DeleteDataObjectClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dataObject = button.DataContext as Syncano.Net.Data.DataObject;
            _viewModel.DeleteDataObject(dataObject);
        }

        private void AddDataObjectClick(object sender, RoutedEventArgs e)
        {
            var dialog = new AddDataObjectWindow();
            if (dialog.ShowDialog() == true)
                _viewModel.AddDataObject(dialog.Title.Text, dialog.Text.Text, dialog.Link.Text, dialog.Image.Text, dialog.Additionals);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _viewModel.Cleanup();
        }

        private void RefreshDataobjectsClick(object sender, RoutedEventArgs e)
        {
            _viewModel.RefreshDataObjects();
        }
    }
}
