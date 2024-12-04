using System.Net;
using TcpServer.Server;
using Sample.PCСomponents;
using TcpServer.ServerDecorator;

namespace Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServerObject baseServer = new ServerObject(IPAddress.Loopback, 8080);
            ITcpServer server = new ConnectionRestrictionDecorator(baseServer, 1);
            server.ServerStarted += () => Console.WriteLine("Сервер запущен.");
            server.ServerStopped += () => Console.WriteLine("Сервер остановлен.");
            server.ClientAdded +=
                (ClientObject client) => Console.WriteLine($"Подключился клиент {client.EndPoint}.");
            server.ClientDisconnected +=
                (ClientObject client) => Console.WriteLine($"Отключился клиент {client.EndPoint}.");
            server.RequestHandler = new RequestHandler();

            _ = Task.Run(server.ListenAsync);
            Console.ReadLine();
            server.Stop();
            Console.WriteLine("Работа программы завершена.");
            Console.ReadLine();
        }
    }
}
