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
        public ClientViewModel()
        {
            ElementsInFolder = new ObservableCollection<ListViewElementModel>();
            client = new Client();
        }

        /// <summary>
        /// Gets files and folders from the directory where the server is running.
        /// </summary>
        public async Task GetFilesAndFoldersFromBaseServersDirectoryAsync(string server, int port)
        { 
            client.Run(server, port);
            var result = await client.List("current");
            FillWithFilesAndFolders(result);
        }

        /// <summary>
        /// Gets files and folders from the one-step upper directory.
        /// </summary>
        public async Task GetFilesFromUpperDirectoryAsync(string server, int port)
        { 
            client.Run(server, port);
            var result = await client.List("..\\");
            FillWithFilesAndFolders(result);
        }

        /// <summary>
        /// Returns a collection that is binded with a ListView that shows files and folders in directory.
        /// </summary>
        public ObservableCollection<ListViewElementModel> ElementsInFolder { get; private set; }

        /// <summary>
        /// Disposes of resourses.
        /// </summary>
        public void Dispose() => client.Dispose();

        /// <summary>
        /// Fills ObservableCollection with the files and folders.
        /// </summary>
        private void FillWithFilesAndFolders(List<(string, bool)> elements)
        {
            ElementsInFolder.Clear();

            foreach (var element in elements)
            {
                ElementsInFolder.Add(new ListViewElementModel(element));
            }
        }
    }
}
