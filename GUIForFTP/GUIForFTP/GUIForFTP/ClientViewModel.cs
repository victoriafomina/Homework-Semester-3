using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SimpleFTPClient;

namespace GUIForFTP
{
    /// <summary>
    /// The class implements view model logic for a client.
    /// </summary>
    public class ClientViewModel : INotifyPropertyChanged
    {
        private int port;
        private string server;
        private Client client;

        /// <summary>
        /// Indicates if the client is connected to the server.
        /// </summary>
        public bool IsConnected = false;

        private string rootServerPath = "1current";

        public string RootClientDirectoryPath;

        private string currentDirectoryOnClientPath;

        private string сurrentDirectoryOnServer;

        private string downloadPath;

        private ObservableCollection<(string, bool)> currentPathsOnServer;

        private ObservableCollection<string> currentPathsOnClient;

        /// <summary>
        /// Stores the information abo
        /// </summary>
        public ObservableCollection<string> ElementsInFolder { get; private set; }

        public ObservableCollection<string> ElementsInfo { get; private set; }

        /// <summary>
        /// Stores the downloads information.
        /// </summary>
        public ObservableCollection<string> DownloadsInfo { get; private set; }

        public delegate void ShowErrorMessage(object sender, string message);

        /// <summary>
        /// Event needed to show errors.
        /// </summary>
        public event ShowErrorMessage ThrowError = (_, __) => { };

        /// <summary>
        /// Event needed to look for properties changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Port.
        /// </summary>
        public string Port
        {
            get
            {
                IsConnected = false;

                return port.ToString();
            }
            set
            {
                IsConnected = false;
                port = Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Server address.
        /// </summary>
        public string Server
        {
            get => server;
            set => server = value;
        }

        /// <summary>
        /// Initializes an instance of the ClientViewModel.
        /// </summary>
        public ClientViewModel(string rootDirectory)
        {
            server = "127.0.0.1";
            port = 6666;
            RootClientDirectoryPath = rootDirectory;
            сurrentDirectoryOnServer = rootServerPath;

            currentDirectoryOnClientPath = RootClientDirectoryPath;
            downloadPath = RootClientDirectoryPath;

            ElementsInFolder = new ObservableCollection<string>();
            ElementsInfo = new ObservableCollection<string>();
            DownloadsInfo = new ObservableCollection<string>();

            InitializeCurrentPathsOnClient();
        }


        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
                => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Connects the client to the server.
        /// </summary>
        public async Task Connect()
        {
            if (IsConnected)
            {
                return;
            }

            client = new Client();

            ElementsInFolder.Clear();

            try
            {
                await Task.Run(() => client.Run(server, port));
                await InitializeCurrentPathsOnServer();
                IsConnected = true;
            }
            catch (Exception e)
            {
                ThrowError(this, e.Message);
            }
        }

        private void InitializeCurrentPathsOnClient()
        {
            currentPathsOnClient = new ObservableCollection<string>();

            currentPathsOnClient.CollectionChanged += CurrentPathsOnClientChanged;

            try
            {
                TryUpdateCurrentPathsOnClient("");
            }
            catch (Exception e)
            {
                ThrowError(this, e.Message);
            }
        }

        private void TryUpdateCurrentPathsOnClient(string folderPath)
        {
            try
            {
                var dirToOpen = Path.Combine(RootClientDirectoryPath, folderPath);

                var folders = Directory.EnumerateDirectories(dirToOpen);

                while (currentPathsOnClient.Count > 0)
                {
                    currentPathsOnClient.RemoveAt(currentPathsOnClient.Count - 1);
                }

                foreach (var folder in folders)
                {
                    var name = folder.Substring(folder.LastIndexOf('\\') + 1);
                    currentPathsOnClient.Add(name);
                }
            }
            catch (Exception e)
            {
                ThrowError(this, e.Message);
            }
        }

        private async Task InitializeCurrentPathsOnServer()
        {
            currentPathsOnServer = new ObservableCollection<(string, bool)>();

            currentPathsOnServer.CollectionChanged += CurrentPathsOnServerChanged;

            await TryUpdateCurrentPathsOnServer(rootServerPath);
        }

        private void CurrentPathsOnClientChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ElementsInfo.Remove(item.ToString());
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ElementsInfo.Add(item.ToString());
                }
            }
        }

        private void CurrentPathsOnServerChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach ((string, bool) pair in e.OldItems)
                {
                    ElementsInFolder.Remove(pair.Item1);
                }
            }

            if (e.NewItems != null)
            {
                foreach ((string, bool) pair in e.NewItems)
                {
                    ElementsInFolder.Add(pair.Item1);
                }
            }
        }

        private bool IsFile(string folderName)
        {
            foreach (var path in currentPathsOnServer)
            {
                if (path.Item1 == folderName)
                {
                    return !path.Item2;
                }
            }

            return false;
        }

        /// <summary>
        /// Opens the folder or downloads the file when the user clicks on ListView item in GUI.
        /// </summary>
        public async Task OpenServerFolderOrDownloadFile(string folderName)
        {
            if (IsFile(folderName))
            {
                await DownloadFile(folderName);

                return;
            }

            if (PathTracker.Path == "")
            {
                await TryUpdateCurrentPathsOnServer(folderName);
                PathTracker.Down(folderName);
            }
            else
            {
                PathTracker.Down(folderName);
                await TryUpdateCurrentPathsOnServer(PathTracker.Path);
            }
        }

        /// <param name="openedFolder">Opened folder.</param>
        private async Task TryUpdateCurrentPathsOnServer(string openedFolder)
        {
            try
            {
                var serverList = await client.List(openedFolder);

                while (currentPathsOnServer.Count > 0)
                {
                    currentPathsOnServer.RemoveAt(currentPathsOnServer.Count - 1);
                }

                foreach (var path in serverList)
                {
                    var name = path.Item1;

                    name = name.Substring(name.LastIndexOf('\\') + 1);

                    currentPathsOnServer.Add((name, path.Item2));
                }

                сurrentDirectoryOnServer = openedFolder;
            }
            catch (Exception e)
            {
                if (e.Message == "-1")
                {
                    ThrowError(this, "Directory not found exception occured");
                    return;
                }

                ThrowError(this, e.Message);
            }
        }

        /// <summary>
        /// Goes one folder upper.
        /// </summary>
        public void FolderUp()
        {
            if (PathTracker.Balance == 0)
            {
                ThrowError(this, "Can not go upper than the root directory!");
                return;
            }

            try
            {
                PathTracker.Up();

                //TryUpdateCurrentPathsOnClient(PathTracker.Path);
                currentDirectoryOnClientPath = Directory.GetParent(currentDirectoryOnClientPath).ToString();
            }
            catch (Exception e)
            {
                ThrowError(this, e.Message);
            }
        }

        public async Task FolderUpServer()
        {
            if (PathTracker.Path == "")
            {
                ThrowError(this, "Can not go upper than the root directory!");
                return;
            }

            try
            {
                PathTracker.Up();

                if (PathTracker.Path == "")
                {
                    await TryUpdateCurrentPathsOnServer("current");
                }
                else
                {
                    await TryUpdateCurrentPathsOnServer(PathTracker.Path);
                }                
            }
            catch (Exception e)
            {
                if (e.Message == "-1")
                {
                    ThrowError(this, "Directory not found!");
                    return;
                }

                ThrowError(this, e.Message);
            }
        }

        /// <summary>
        /// Downloads a file.
        /// </summary>
        public async Task DownloadFile(string fileName)
        {
            try
            {
                var pathToFile = Path.Combine(сurrentDirectoryOnServer, fileName);

                DownloadsInfo.Add($"Downloading: {fileName}");

                await client.Get(pathToFile, downloadPath);

                DownloadsInfo.Add($"Downloaded: {fileName}");
            }
            catch (Exception e)
            {
                ThrowError(this, e.Message);
            }
        }

        /// <summary>
        /// Downloads all files in curr
        /// </summary>
        public async Task DownloadAllFiles()
        {
            try
            {
                foreach (var path in currentPathsOnServer)
                {
                    if (!path.Item2)
                    {
                        await DownloadFile(path.Item1);
                    }
                }
            }
            catch (Exception e)
            {
                ThrowError(this, e.Message);
            }
        }
    }
}
