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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SimpleFTPClient;

namespace GUIForFTP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClientViewModel clientViewModel;

        public MainWindow()
        {
            InitializeComponent();
            clientViewModel = new ClientViewModel();
            elementsInFolder.ItemsSource = clientViewModel.ElementsInFolder;
            downloadsInfo.ItemsSource = clientViewModel.DownloadsInfo;
            InitFillFilesAndFoldersListView();
        }

        private void ConnectServer_Click(object sender, RoutedEventArgs e)
        {
            //var correct = true;

            //if (Uri.CheckHostName(server.Text) == UriHostNameType.Unknown)
            //{
            //    MessageBox.Show("Host name is not correct!");
            //    correct = false;
            //}

            //if (!int.TryParse(port.Text, out _))
            //{
            //    MessageBox.Show("Port must be a numeral value!");
            //    correct = false;
            //}

            //if ((sender as Button).Background == Brushes.Red)
            //{
            //    (sender as Button).Background = Brushes.Green;
            //    (sender as Button).Content = "Run";
            //    clientViewModel.Dispose();
            //    MessageBox.Show("Client disconnected from server!");
            //}
            //else if (correct)
            //{
            //    (sender as Button).Background = Brushes.Red;
            //    (sender as Button).Content = "Stop";
            //    clientViewModel.
            //    MessageBox.Show("Client connected to server!");
            //}
        }

        private bool CheckServerCorectness() =>
                Uri.CheckHostName(server.Text) != UriHostNameType.Unknown;

        private bool CheckPortCorectness() => int.TryParse(port.Text, out _);

        private void PortOrHostNameIsIncorrect() => MessageBox.Show("Port or hostname is incorrect!");

        private async void FolderUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckPortCorectness() && CheckServerCorectness())
                {
                    await clientViewModel.GetFilesFromUpperDirectoryAsync(server.Text, int.Parse(port.Text));

                    return;
                }

                PortOrHostNameIsIncorrect();  
            }
            catch (AccessToDirectoryOnServerDenied)
            {
                MessageBox.Show("You don't have an access to upper directories!");
            }
        }

        private void ElementsInFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as ListViewElementModel;

            if (item != null)
            {
                MessageBox.Show($"Item: {item}");
            }
        }

        private void SaveAllFilesInFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void InitFillFilesAndFoldersListView() =>
                await clientViewModel.GetFilesAndFoldersFromBaseServersDirectoryAsync("127.0.0.1", 6666);
    }
}
