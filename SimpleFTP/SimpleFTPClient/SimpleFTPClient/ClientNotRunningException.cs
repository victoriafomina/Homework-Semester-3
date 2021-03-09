using System;

namespace SimpleFTPClient
{
    /// <summary>
    /// The exception that is thrown when the client is not running while trying to make requests.
    /// </summary>
    [Serializable]
    public class ClientNotRunningException : Exception { }
}
