using System;

namespace GUIForFTP
{
    /// <summary>
    /// The exception is thrown when the user tries to access a directory that is upper than a project root directory on a server.
    /// </summary>
    [Serializable]
    public class CouldNotAccessDirUpperThanRootException : Exception { }
}
