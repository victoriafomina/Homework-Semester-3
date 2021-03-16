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

        public string RootClientDirectoryPath;

        private string currentDirectoryOnClientPath;

        private string сurrentDirectoryOnServer;

        private string currentDirectoryOnClient;

        private string downloadPath;

        public string DownloadFolder
        {
            get
            {
                var tmp = downloadPath.Remove(downloadPath.Length - 1);
                return tmp.Substring(tmp.LastIndexOf('\\') + 1);
            }
            set
            {
                downloadPath = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<(string, bool)> currentPathsOnServer;

        private ObservableCollection<string> currentPathsOnClient;

        public ObservableCollection<string> DisplayedListOnServer { get; private set; }

        public ObservableCollection<string> ElementsInfo { get; private set; }

        public ObservableCollection<string> DownloadsInProgressList { get; private set; }

        public ObservableCollection<string> DownloadsFinishedList { get; private set; }

        public delegate void ShowErrorMessage(object sender, string message);

        public event ShowErrorMessage ThrowError = (_, __) => { };

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
        public ClientViewModel(string rootClientDirectory)
        {
            server = "127.0.0.1";
            port = 6666;
            RootClientDirectoryPath = rootClientDirectory;
            сurrentDirectoryOnServer = "current";

            currentDirectoryOnClientPath = RootClientDirectoryPath;
            currentDirectoryOnClient = "";
            downloadPath = RootClientDirectoryPath;

            DisplayedListOnServer = new ObservableCollection<string>();
            ElementsInfo = new ObservableCollection<string>();
            DownloadsInProgressList = new ObservableCollection<string>();
            DownloadsFinishedList = new ObservableCollection<string>();

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

            DisplayedListOnServer.Clear();

            try
            {
                client.Run(server, port);
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

        private async Task InitializeCurrentPathsOnServer()
        {
            currentPathsOnServer = new ObservableCollection<(string, bool)>();

            currentPathsOnServer.CollectionChanged += CurrentPathsOnServerChanged;

            await TryUpdateCurrentPathsOnServer("current");
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
                    DisplayedListOnServer.Remove(pair.Item1);
                }
            }

            if (e.NewItems != null)
            {
                foreach ((string, bool) pair in e.NewItems)
                {
                    DisplayedListOnServer.Add(pair.Item1);
                }
            }
        }

        public void OpenClientFolder(string folderName)
        {
            var nextDirectoryPath = Path.Combine(currentDirectoryOnClientPath, folderName);

            if (Directory.Exists(nextDirectoryPath))
            {
                var nextDirectoryOnClient = Path.Combine(currentDirectoryOnClient, folderName);
                TryUpdateCurrentPathsOnClient(nextDirectoryOnClient);
                currentDirectoryOnClientPath = nextDirectoryPath;
                currentDirectoryOnClient = Path.Combine(currentDirectoryOnClient, folderName);
            }
            else
            {
                ThrowError(this, "Directory not found");
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

            string nextDirectory;

            if (PathTracker.Path == "")
            {
                nextDirectory = folderName;
                PathTracker.Down(folderName);
            }
            else
            {
                PathTracker.Down(folderName);
                nextDirectory = PathTracker.Path;
            }

            await TryUpdateCurrentPathsOnServer(nextDirectory);
        }
        
        /// <param name="folderPath">Opened folder.</param>
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
            if (currentDirectoryOnClient == "")
            {
                ThrowError(this, "Can't go back from the root directory");
                return;
            }

            try
            {
                var index = currentDirectoryOnClient.LastIndexOf('\\');
                string toOpen;

                if (index > 0)
                {
                    toOpen = currentDirectoryOnClient.Substring(0, currentDirectoryOnClient.LastIndexOf('\\'));
                }
                else
                {
                    toOpen = "";
                }

                TryUpdateCurrentPathsOnClient(toOpen);
                currentDirectoryOnClient = toOpen;
                currentDirectoryOnClientPath = Directory.GetParent(currentDirectoryOnClientPath).ToString();
            }
            catch (Exception e)
            {
                ThrowError(this, e.Message);
            }
        }

        /// <summary>
        /// Возвращение назад на сервере
        /// </summary>
        public async Task GoBackServer()
        {
            if (PathTracker.Path == "")
            {
                ThrowError(this, "Can't go back from the root directory");
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
                    ThrowError(this, "Directory not found exception occured");
                    return;
                }

                ThrowError(this, e.Message);
            }
        }

        public void UpdateDownloadFolder()
        {
            if (downloadPath == currentDirectoryOnClientPath)
            {
                return;
            }

            DownloadFolder = currentDirectoryOnClientPath + "\\";
        }

        public async Task DownloadFile(string fileName)
        {
            try
            {
                var pathToFile = Path.Combine(сurrentDirectoryOnServer, fileName);

                DownloadsInProgressList.Add(fileName);

                await client.Get(pathToFile, downloadPath);

                DownloadsInProgressList.Remove(fileName);
                DownloadsFinishedList.Add(fileName);
            }
            catch (Exception e)
            {
                ThrowError(this, e.Message);
            }
        }

        public async Task DownloadAllFilesInCurrentDirectory()
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
