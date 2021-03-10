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

        private string Path { get; set; }

        private bool IsDirectory { get; set; }

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
