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
            DownloadsInfo = new ObservableCollection<string>();
            client = new Client();
        }

        /// <summary>
        /// Gets files and folders from the one-step upper directory.
        /// </summary>
        public async Task GetListOfFilesAndFoldersFromDirectoryAsync(Direction direction, string dir, string server, int port)
        {
            client.Run(server, port);
            List<(string, bool)> result;

            if (Direction.Current == direction)
            {
                result = await client.List("current");
                FillWithFilesAndFolders(result);
            }
            else if (Direction.Down == direction)
            {
                PathTracker.Down(dir);
                var path = PathTracker.Path;
                result = await client.List($"{path}");
                FillWithFilesAndFolders(result);
            }
            else if (PathTracker.Balance > 0)
            {
                PathTracker.Up();
                var path = PathTracker.Path;
                result = await client.List($"{path}");
                FillWithFilesAndFolders(result);
            }
            else
            {
                throw new CouldNotAccessDirUpperThanRootException();
            }
        }

        /// <summary>
        /// Downloads file from server.
        /// </summary>
        public async Task DownloadFileFromServer(string downloadFrom, string downloadTo) =>
                await client.Get(downloadFrom, downloadTo);

        /// <summary>
        /// Returns a collection that is binded with a ListView that shows files and folders in directory.
        /// </summary>
        public ObservableCollection<ListViewElementModel> ElementsInFolder { get; private set; }

        /// <summary>
        /// Returns a collection that is binded with a ListView that shows downloads information.
        /// </summary>
        public ObservableCollection<string> DownloadsInfo { get; private set; }

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

    /// <summary>
    /// Contains three possible directions in the folder tracking.
    /// </summary>
    public enum Direction
    {
        Up,
        Down,
        Current
    }
}
