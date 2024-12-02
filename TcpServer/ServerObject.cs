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
        public IRequestHandler RequestHandler { get; set; }

        public event Action ServerStarted;
        public event Action<TcpClient> ClientAdded;
        public event Action<TcpClient> ClientDisconnected;
        public event Action ServerStopped;

        protected TcpListener _listener;
        protected CancellationTokenSource _cts;
        protected List<ClientObject> _clients;

        public ServerObject(string ip, int port)
        {
            EndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            _listener = new TcpListener(EndPoint);
            _cts = new CancellationTokenSource();
            _clients = new List<ClientObject>();
        }
        public async Task ListenAsync()
        {
            try
            {
                _listener.Start();

                ServerStarted?.Invoke();

                var token = _cts.Token;
                while (!token.IsCancellationRequested)
                {
                    var tcpClient = await _listener.AcceptTcpClientAsync();
                    var client = new ClientObject(tcpClient, RequestHandler, this);
                    _clients.Add(client);

                    ClientAdded?.Invoke(tcpClient);

                    _ = Task.Run(() => client.ClientHandlerAsync(token), token);
                }
            }
            catch (Exception) { throw; }
            finally { Disconnect(); }
        }
        public void RemoveClient(ClientObject client)
        {
            _clients.Remove(client);
            client.Close();

            ClientDisconnected?.Invoke(client.Client);
        }
        public void Stop()
        {
            _cts.Cancel();
        }
        private void Disconnect()
        {
            foreach (var client in _clients)
            {
                client.Close();
            }
            _listener.Stop();
            _clients.Clear();

            ServerStopped?.Invoke();
        }
    }
}
