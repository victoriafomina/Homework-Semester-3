using System;

namespace SimpleFTPClient
{
    /// <summary>
    /// The exception that is thrown when the response body is invalid.
    /// </summary>
    [Serializable]
    public class InvalidResponseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidResponseException class with the specified error message.
        /// </summary>
        public InvalidResponseException(string response) : base(response) { }
    }
}
