using System.Windows;
using System.Windows.Controls;
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
            model = new ClientViewModel("E:\\");

            model.ThrowError += (sender, message) => ShowMessage(message);

            DataContext = model;

            InitializeComponent();
        }

        private async void HandleDouble_Click(object sender, RoutedEventArgs e) =>
                await model.OpenServerFolderOrDownloadFile((sender as ListViewItem).Content.ToString());

        private void ShowMessage(string errorMessage) =>  MessageBox.Show(errorMessage, "Error message");

        private async void Connect_Click(object sender, RoutedEventArgs e) => await model.Connect();

        private async void DownloadAll_Click(object sender, RoutedEventArgs e) => await model.DownloadAllFiles();

        private async void FolderUp_Click(object sender, RoutedEventArgs e) => await model.FolderUpServer();

        private void addressTextBox_TextChanged(object sender, TextChangedEventArgs e) => model.IsConnected = false;

        private void portTextBox_TextChanged(object sender, TextChangedEventArgs e) => model.IsConnected = false;
    }
}
