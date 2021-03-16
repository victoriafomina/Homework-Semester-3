using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SimpleFTPClient;

namespace GUIForFTP
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClientViewModel model;

        public MainWindow()
        {
            model = new ClientViewModel("..\\..\\..\\Client\\res\\Downloads\\");

            model.ThrowError += (sender, message) => ShowMessage(message);

            DataContext = model;

            InitializeComponent();
        }
        private async void HandleServerDoubleClick(object sender, RoutedEventArgs e)
        {
            await model.OpenServerFolderOrDownloadFile((sender as ListViewItem).Content.ToString());
        }

        private void HandleClientDoubleClick(object sender, RoutedEventArgs e)
        {
            model.OpenClientFolder((sender as ListViewItem).Content.ToString());
        }

        private void ShowMessage(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Error occured");
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            await model.Connect();
        }

        private async void DownloadAll_Click(object sender, RoutedEventArgs e)
        {
            await model.DownloadAllFilesInCurrentDirectory();
        }

        private async void FolderUp_Click(object sender, RoutedEventArgs e)
        {
            await model.GoBackServer();
        }

        private void BackClient_Click(object sender, RoutedEventArgs e)
        {
            model.FolderUp();
        }

        private void ChooseFolder_Click(object sender, RoutedEventArgs e)
        {
            model.UpdateDownloadFolder();
        }

        private void addressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            model.IsConnected = false;
        }

        private void portTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            model.IsConnected = false;
        }
    }
}
