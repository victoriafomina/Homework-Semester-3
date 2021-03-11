using System;

namespace SimpleFTPClient
{
    /// <summary>
    /// The exception that is thrown when the client is not running when trying to make a request.
    /// </summary>
    [Serializable]
    public class ClientNotRunningException : Exception { }
}
