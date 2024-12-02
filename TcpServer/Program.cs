using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpServer
{
    public class Server
    {
        public TcpListener listener { get; set; }

        public Server(IPAddress address, int port)
        {
            IPEndPoint endPoint = new IPEndPoint(address, port);
            listener = new TcpListener(endPoint);
        }
        public async Task Listen()
        {
            try
            {
                listener.Start();
                Console.WriteLine("Сервер запущен!");

                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => ProcessAsync(client));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                listener.Stop();
            }
        }
        public async Task ProcessAsync(TcpClient client)
        {
            try
            {
                Console.WriteLine("Подключился новый пользователь.");
                NetworkStream stream = client.GetStream();

                List<byte> buffer = new List<byte>();
                var byteRead = new byte[1];
                while (true)
                {
                    await stream.ReadAtLeastAsync(byteRead, 1, false);
                    Console.WriteLine("Байт считан.");
                    buffer.Add(byteRead[0]);
                    if (byteRead[0] == '\n')
                    {
                        var message = Encoding.ASCII.GetString(buffer.ToArray());
                        Console.WriteLine("Получено сообщение:");
                        Console.Write(message);

                        buffer.Clear();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                client.Close();
                Console.WriteLine("Пользователь отключился.");
            }
        }
    }
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Server server = new Server(IPAddress.Any, 8080);
            await server.Listen();
        }
    }
}
