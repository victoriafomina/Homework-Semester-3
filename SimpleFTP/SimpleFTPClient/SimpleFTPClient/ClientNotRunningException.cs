using System;

namespace SimpleFTPClient
{
    /// <summary>
    /// The exception that is thrown when the client is not running.
    /// </summary>
    [Serializable]
    public class ClientNotRunningException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ClientNotRunningException class.
        /// </summary>
        public ClientNotRunningException() : base() { }
    }
}
