using System;
using System.IO;
using System.Threading.Tasks;
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

        private bool CheckServerAndPortCorectness() =>
                Uri.CheckHostName(server.Text) != UriHostNameType.Unknown && int.TryParse(port.Text, out _);

        private async void FolderUp_Click(object sender, RoutedEventArgs e)
        {   if (CheckServerAndPortCorectness())
            {
                try
                {
                    await clientViewModel.GetListOfFilesAndFoldersFromDirectoryAsync(Direction.Up, "", server.Text, int.Parse(port.Text));
                }
                catch (CouldNotAccessDirUpperThanRootException)
                {
                    MessageBox.Show("You don't have an access to upper directories!");
                }

                return;
             }

             MessageBox.Show("Port and/or hostname are incorrect!");
        }

        private async void ElementsInFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)e.OriginalSource).DataContext as ListViewElementModel;

            var isCorrectServerAndPort = CheckServerAndPortCorectness();

            if (isCorrectServerAndPort && item.IsDirectory)
            {
                await clientViewModel.GetListOfFilesAndFoldersFromDirectoryAsync(Direction.Down, item.Path, server.Text, int.Parse(port.Text));
            }
            else if (isCorrectServerAndPort && Directory.Exists(downloadTo.Text))
            {
                clientViewModel.DownloadsInfo.Add($"Trying to start downloading \"{item.Path}\"");
                await clientViewModel.DownloadFileFromServer(item.Path, downloadTo.Text);
                clientViewModel.DownloadsInfo.Add($"\"{item.Path}\" downloaded");
                MessageBox.Show($"\"{item.Path}\" downloaded");
            }
            else
            { 
                if (!isCorrectServerAndPort)
                {
                    MessageBox.Show("Port and/or hostname are incorrect!");
                }

                if (!Directory.Exists(downloadTo.Text))
                {
                    MessageBox.Show("Directory you want to download to does not exist!");
                }
            }    
        }

        private async void SaveAllFilesInFolder_Click(object sender, RoutedEventArgs e)
        {
            if (CheckServerAndPortCorectness())
            {
                foreach (var file in clientViewModel.ElementsInFolder)
                {
                    if (!file.IsDirectory)
                    {
                        clientViewModel.DownloadsInfo.Add($"Trying to start downloading \"{file.Path}\"");
                        await clientViewModel.DownloadFileFromServer(file.Path, downloadTo.Text);
                        clientViewModel.DownloadsInfo.Add($"\"{file.Path}\" downloaded");
                        MessageBox.Show($"\"{file.Path}\" downloaded");
                    }
                }

                return;
            }

            MessageBox.Show("Port and/or hostname are incorrect!");
        }

        private async void InitFillFilesAndFoldersListView() =>
                await clientViewModel.GetListOfFilesAndFoldersFromDirectoryAsync(Direction.Current, "", "127.0.0.1", 6666);
    }
}
