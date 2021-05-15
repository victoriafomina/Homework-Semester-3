using System;

namespace SimpleFTPClient
{
    /// <summary>
    /// Exception is thrown when the client writer gets "denied" string from server.
    /// </summary>
    [Serializable]
    public class AccessToDirectoryOnServerDeniedException : Exception { }
}
