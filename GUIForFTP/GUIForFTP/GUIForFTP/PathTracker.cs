namespace GUIForFTP
{
    /// <summary>
    /// Tracks the user movements between folders on a server.
    /// </summary>
    public static class PathTracker
    {
        /// <summary>
        /// Returns balance value of a directory.
        /// 0 - balance of a root server directory.
        /// The greater balance the bigger nesting level of folders.
        /// </summary>
        public static int Balance { get; private set; } = 0;

        /// <summary>
        /// Returns path of a current directory the user observing on a server.
        /// </summary>
        public static string Path { get; private set; } = "";

        /// <summary>
        /// Returns current folder.
        /// </summary>
        public static string CurrentFolder
        {
            get
            {
                if (Balance <= 1)
                {
                    return Path;
                }
                else
                {
                    var lastSlashIndex = Path.IndexOf("\\");

                    return Path.Substring(0, Path.Length - lastSlashIndex - 1);
                }
            }
        }

        /// <summary>
        /// Goes one-folder upper.
        /// </summary>
        public static void Up()
        {
            if (Balance == 1)
            {
                Path = "";
                --Balance;

                return;
            }
            else if (Balance > 0)
            {
                var index = Path.LastIndexOf("\\");
                Path = Path.Remove(index);
                --Balance;

                return;
            }

            throw new CouldNotAccessDirUpperThanRootException();
        }

        /// <summary>
        /// Goes one-folder below.
        /// </summary>
        public static void Down(string dirName)
        {
            if (Balance > 0)
            {
                Path += "\\";
            }

            Path += dirName;
            ++Balance;
        }
    }
}
