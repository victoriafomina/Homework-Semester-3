
namespace SimpleFTPServer
{
    public class Program
    {
        public static async System.Threading.Tasks.Task Main(string[] args)
        {
            var server = new Server(6666);
            await server.Run();
        }        
    }
}
