using System.Net;
using System.Net.Sockets;
using System.Text;
using TcpServer.Server;

namespace TcpServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServerObject server = new ServerObject(IPAddress.Any, 8080);
            server.ServerStarted += () => Console.WriteLine("Сервер запущен.");
            server.ServerStopped += () => Console.WriteLine("Сервер остановлен.");
            server.ClientAdded += 
                (ClientObject client) => Console.WriteLine($"Подключился клиент {client.EndPoint}.");
            server.ClientDisconnected += 
                (ClientObject client) => Console.WriteLine($"Отключился клиент {client.EndPoint}.");
            server.RequestHandler = new PCСomponents.RequestHandler();

            _ = Task.Run(server.ListenAsync);
            Console.ReadLine();
            server.Stop();
            Console.WriteLine("Работа программы завершена.");
            Console.ReadLine();
        }
    }
}
