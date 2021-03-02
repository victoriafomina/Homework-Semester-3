using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFTPServer
{
    /// <summary>
    /// Handles clients' requests.
    /// </summary>
    public static class RequestHandler
    {
        /// <summary>
        /// Handles the client's request.
        /// </summary>
        public static async Task HandleRequest(string request, StreamWriter writer)
        {
            (int, string) parsedRequest;
            var errorMessage = "Invalid request body!";

            try
            {
                parsedRequest = Parse(request);
            }
            catch (InvalidRequestBodyException)
            {
                await writer.WriteLineAsync(errorMessage);
                return;
            }

            var root = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "res\\");
            var path = Path.Combine(root, parsedRequest.Item2);

            switch (parsedRequest.Item1)
            {
                case 1:
                    var delta = root.Length;
                    await List(path, delta, writer);
                    break;
                case 2:
                    await Get(path, writer);
                    break;
                default:
                    await writer.WriteLineAsync(errorMessage);
                    return;
            }
        }

        /// <summary>
        /// Makes listing (listing files in the server's directory).
        /// </summary>
        private static async Task List(string path, int delta, StreamWriter writer)
        {
            if (!Directory.Exists(path))
            {
                await writer.WriteLineAsync("-1");
                return;
            }

            var response = new StringBuilder();

            var files = Directory.GetFiles(path);
            var folders = Directory.GetDirectories(path);

            var responseSize = files.Length + folders.Length;

            response.Append($"{responseSize} ");

            foreach (var file in files)
            {
                var formattedName = file.Remove(0, delta);
                response.Append($".\\{formattedName} false ");
            }

            foreach (var folder in folders)
            {
                var formattedName = folder.Remove(0, delta);
                response.Append($".\\{formattedName} true ");
            }

            await writer.WriteLineAsync(response.ToString());
        }

        /// <summary>
        /// Gets a file from the server's directory.
        /// </summary>
        private static async Task Get(string path, StreamWriter writer)
        {
            if (!File.Exists(path))
            {
                await writer.WriteLineAsync("-1");
                return;
            }

            var size = new FileInfo(path).Length;
            await writer.WriteLineAsync($"{size} ");

            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                await fileStream.CopyToAsync(writer.BaseStream);
            }
        }

        /// <summary>
        /// Parses string into tuple(int x , string y). Where x is a request type number, y is a path.
        /// </summary>
        /// <exception cref="InvalidRequestBodyException">Thrown when request body is invalid.</exception>
        public static (int, string) Parse(string request)
        {
            var errorMessage = "Invalid request body!";

            if (request == null || request.Length < 3)
            {
                throw new InvalidRequestBodyException(errorMessage);
            }

            (int, string) parsedRequest;

            if (int.TryParse(request[0].ToString(), out parsedRequest.Item1))
            {
                parsedRequest.Item2 = request.Substring(1);

                return parsedRequest;
            }

            throw new InvalidRequestBodyException(errorMessage);
        }
    }
}
