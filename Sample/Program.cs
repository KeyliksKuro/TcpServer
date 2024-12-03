using System.Net;
using TcpServer.Server;
using Sample.PCСomponents;

namespace Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServerObject server = new ServerObject(IPAddress.Loopback, 8080);
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
