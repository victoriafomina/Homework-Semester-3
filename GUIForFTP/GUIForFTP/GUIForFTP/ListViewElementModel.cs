namespace GUIForFTP
{
    /// <summary>
    /// Class implements structure for binding ListView element with a ObservableCollection element.
    /// </summary>
    public class ListViewElementModel
    {
        /// <summary>
        /// Initializes an object of ListViewElementModel.
        /// </summary>
        public ListViewElementModel((string, bool) element)
        {
            Path = element.Item1;
            IsDirectory = element.Item2;
        }

        /// <summary>
        /// Path to the element.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Checks if the elements is a directory or a file.
        /// </summary>
        public bool IsDirectory { get; private set; }

        /// <summary>
        /// Returns list view element value in a string.
        /// </summary>
        public override string ToString()
        {
            if (IsDirectory)
            {
                return $"Directory: {Path}";
            }

            return $"File: {Path}";
        }
    }
}
