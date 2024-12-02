using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
    internal class ServerObject
    {
        public IPEndPoint EndPoint { get; set; }
        public int Port { get; set; }
        public bool Active { get; set; }

        private Socket _listener; // представляет объект, который ведет прослушивание
        private volatile CancellationTokenSource _cts; // токен отменты, с помощью него будут останавливаться потоки при остановке сервера

        public ServerObject(string ip, int port)
        {
            Port = port;
            EndPoint = new IPEndPoint(IPAddress.Parse(ip), Port);
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _cts = new CancellationTokenSource();
        }
        public void Start()
        {
            if (Active)
            {
                Console.WriteLine("Server was started");
                return;
            }
            _listener.Bind(EndPoint);
            _listener.Listen(16);
            Active = true;

            while (Active || !_cts.Token.IsCancellationRequested)
            {
                try
                {
                    Socket listenerAccept = _listener.Accept();
                    if (listenerAccept != null)
                    {
                        Task.Run(
                          () => ClientThread(listenerAccept),
                          _cts.Token
                        );
                    }
                }
                catch { }
            }
        }
        public void Stop()
        {
            if (!Active)
            {
                Console.WriteLine("Server was stopped");
                return;
            }
            _cts.Cancel();
            _listener.Close();
            Active = false;

        }
        public void ClientThread(Socket client)
        {
            new Client(client);
        }
    }
}
