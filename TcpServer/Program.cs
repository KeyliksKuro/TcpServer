using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ServerObject server = new ServerObject("127.0.0.1", 8080);
        }
    }
}
