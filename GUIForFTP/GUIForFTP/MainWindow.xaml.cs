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
            InitFillFilesAndFoldersListView();
        }

        private void ConnectServer_Click(object sender, RoutedEventArgs e)
        {
            var correct = true;

            if (Uri.CheckHostName(server.Text) == UriHostNameType.Unknown)
            {
                MessageBox.Show("Host name is not correct!");
                correct = false;
            }

            if (!int.TryParse(port.Text, out _))
            {
                MessageBox.Show("Port must be a numeral value!");
                correct = false;
            }

            if ((sender as Button).Background == Brushes.Red)
            {
                (sender as Button).Background = Brushes.Green;
                (sender as Button).Content = "Run";
                clientViewModel.Dispose();
                MessageBox.Show("Client disconnected from server!");
            }
            else if (correct)
            {
                (sender as Button).Background = Brushes.Red;
                (sender as Button).Content = "Stop";
                clientViewModel.ConnectServer();
                MessageBox.Show("Client connected to server!");
            }
        }

        private void FolderUp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveAllFilesInFolder_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void InitFillFilesAndFoldersListView()
        {
            clientViewModel = new ClientViewModel("127.0.0.1", 6666);
            elementsInFolder.ItemsSource = clientViewModel.ElementsInFolder;
            clientViewModel.ConnectServer();
            await clientViewModel.GetFilesAndFoldersFromBaseServersDirectoryAsync();
        }
    }
}
