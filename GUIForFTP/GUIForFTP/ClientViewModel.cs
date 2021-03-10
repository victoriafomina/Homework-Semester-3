using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SimpleFTPClient;

namespace GUIForFTP
{
    /// <summary>
    /// The class implements view model logic for a client.
    /// </summary>
    public class ClientViewModel : IDisposable
    {
        private Client client;

        /// <summary>
        /// Initializes an instance of the ClientViewModel.
        /// </summary>
        public ClientViewModel(string server, int port)
        {
            ElementsInFolder = new ObservableCollection<ListViewElementModel>();
            client = new Client(server, port);
        }

        /// <summary>
        /// Gets files and folders from the directory where the server is running.
        /// </summary>
        public async Task GetFilesAndFoldersFromBaseServersDirectoryAsync()
        {
            client.Run();
            var result = await client.List("current");
            FillElementsInFolder(result);
        }        

        /// <summary>
        /// Connects the client with a server.
        /// </summary>
        public void ConnectServer() => client.Run();

        /// <summary>
        /// Returns a collection that is binded with a ListView that shows files and folders in directory.
        /// </summary>
        public ObservableCollection<ListViewElementModel> ElementsInFolder { get; private set; }

        /// <summary>
        /// Disposes of resourses.
        /// </summary>
        public void Dispose() => client.Dispose();

        private void FillElementsInFolder(List<(string, bool)> elements)
        {
            ElementsInFolder.Clear();

            foreach (var element in elements)
            {
                ElementsInFolder.Add(new ListViewElementModel(element));
            }
        }
    }
}
