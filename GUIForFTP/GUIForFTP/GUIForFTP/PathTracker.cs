namespace GUIForFTP
{
    /// <summary>
    /// Tracks the user movements between folders on a server.
    /// </summary>
    public static class PathTracker
    {
        /// <summary>
        /// Returns nesting level of the directory.
        /// 0 - balance of the root server directory.
        /// The greater value the bigger nesting level of folders.
        /// </summary>
        public static int NestingLevel { get; private set; } = 0;

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
                if (NestingLevel <= 1)
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
            if (NestingLevel == 1)
            {
                Path = "";
                --NestingLevel;

                return;
            }
            else if (NestingLevel > 0)
            {
                var index = Path.LastIndexOf("\\");
                Path = Path.Remove(index);
                --NestingLevel;

                return;
            }

            throw new CouldNotAccessDirUpperThanRootException();
        }

        /// <summary>
        /// Goes one-folder below.
        /// </summary>
        public static void Down(string dirName)
        {
            if (NestingLevel > 0)
            {
                Path += "\\";
            }

            Path += dirName;
            ++NestingLevel;
        }
    }
}
