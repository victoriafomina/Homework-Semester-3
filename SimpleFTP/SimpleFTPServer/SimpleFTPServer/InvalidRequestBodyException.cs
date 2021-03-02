using System;

namespace SimpleFTPServer
{
    /// <summary>
    /// The exception that is thrown when the request body is invalid.
    /// </summary>
    [Serializable]
    public class InvalidRequestBodyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the InvalidRequestBodyException class with the specified error message.
        /// </summary>
        public InvalidRequestBodyException(string message) : base(message) { }
    }
}
