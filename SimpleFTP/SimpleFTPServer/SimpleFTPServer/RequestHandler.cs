using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleFTPServer
{
    /// <summary>
    /// The class was implemented to handle clients' requests.
    /// </summary>
    public static class RequestHandler
    {
        /// <summary>
        /// Handles the client's request.
        /// </summary>
        public static void HandleRequest(string request, StreamWriter writer)
        {
            if (!RequestParser.IsCorrectRequestFormat(request))
            {
                Console.WriteLine("Invalid request body!");
                return;
            }

            var parsedRequest = RequestParser.Parse(request);

            switch (parsedRequest.Item1)
            {
                case 1:
                    List(parsedRequest.Item2);
                    break;
                case 2:
                    Get(parsedRequest.Item2);
                    break;
                default:
                    throw new InvalidRequestBodyException("Invalid request body!");
            }
        }

        private static string List(string path)
        {

            return "";
        }
        private static string Get(string path)
        {

            return "";
        }
    }
}
