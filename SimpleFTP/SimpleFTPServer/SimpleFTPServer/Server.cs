using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace SimpleFTPServer
{
    /// <summary>
    /// Server that handles two requests: List (listing files of the server's directory) and Get (saving
    /// file from server's directory)
    /// </summary>
    public class Server
    {
        public Server(int port)
        {

        }

        private UdpClient udpClient;
    }
}
