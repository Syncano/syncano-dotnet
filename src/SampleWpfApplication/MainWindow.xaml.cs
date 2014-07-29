using System.Windows;
using MahApps.Metro.Controls;

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

        private void AddDataObjectHttpClick(object sender, RoutedEventArgs e)
        {
            var dialog = new AddDataObjectWindow();
            if (dialog.ShowDialog() == true)
                _viewModel.AddDataObjectHttp(dialog.Title.Text, dialog.Text.Text, dialog.Link.Text, dialog.Image.Text, dialog.Additionals);
        }

        private void AddDataObjectSyncClick(object sender, RoutedEventArgs e)
        {
            var dialog = new AddDataObjectWindow();
            if (dialog.ShowDialog() == true)
                _viewModel.AddDataObjectSync(dialog.Title.Text, dialog.Text.Text, dialog.Link.Text, dialog.Image.Text, dialog.Additionals);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _viewModel.Cleanup();
        }

        private void RefreshDataobjectsClick(object sender, RoutedEventArgs e)
        {
            _viewModel.RefreshDataObjectsHttp();
        }
    }
}
