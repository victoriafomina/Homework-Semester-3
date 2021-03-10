using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
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

        private bool CheckServerAndPortCorectness() =>
                Uri.CheckHostName(server.Text) != UriHostNameType.Unknown && int.TryParse(port.Text, out _);

        private void CheckAndHandlePortAndHostNameCorectness()
        {
            if (CheckServerAndPortCorectness())
            {
                MessageBox.Show("Port or hostname is incorrect!");
            }
        }

        private async void FolderUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckServerAndPortCorectness())
                {
                    try
                    {
                        await clientViewModel.GetListOfFilesAndFoldersFromDirectoryAsync(Direction.Up, "", server.Text, int.Parse(port.Text));
                    }
                    catch (AccessToDirectoryOnServerDeniedException)
                    {
                        MessageBox.Show("You don't have an access to upper directories!");
                    }

                    return;
                }

                MessageBox.Show("Port and/or hostname are incorrect!");
            }
            catch (AccessToDirectoryOnServerDeniedException)
            {
                MessageBox.Show("You don't have an access to upper directories!");
            }
        }

        private async void ElementsInFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as ListViewElementModel;

            if (item.IsDirectory && CheckServerAndPortCorectness())
            {
                await clientViewModel.GetListOfFilesAndFoldersFromDirectoryAsync(Direction.Down, item.Path, server.Text, int.Parse(port.Text));
            }
            else if (CheckServerAndPortCorectness() && Directory.Exists(downloadTo.Text))
            {
                clientViewModel.DownloadsInfo.Add($"Trying to start to download \"{item.Path}\"");
                await clientViewModel.DownloadFileFromServer(item.Path, downloadTo.Text);
                clientViewModel.DownloadsInfo.Add($"\"{item.Path}\" downloaded");
            }
            else
            {
                // message box (port and host name bla bla bla) or (ne sushetv directoriya)
            }    
        }

        private void SaveAllFilesInFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void InitFillFilesAndFoldersListView() =>
                await clientViewModel.GetListOfFilesAndFoldersFromDirectoryAsync(Direction.Current, "", "127.0.0.1", 6666);
    }
}
