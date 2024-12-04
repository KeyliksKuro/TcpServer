using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TcpServer.Server;

namespace TcpServer.ServerDecorator
{
    public class ConnectionRestrictionDecorator : TcpServerDecorator
    {
        public int MaxConnections { get; set; }
        public int CurrentConnections { get; set; }
        public override IPEndPoint EndPoint { get => _server.EndPoint; }
        public override IRequestHandler? RequestHandler { get => _server.RequestHandler; set => _server.RequestHandler = value; }

        public ConnectionRestrictionDecorator(ServerObject server, int maxConnections) : base(server)
        {
            MaxConnections = maxConnections;
            CurrentConnections = 0;
        }

        public override event Action ServerStarted;
        public override event Action<ClientObject> ClientAdded;
        public override event Action<ClientObject> ClientDisconnected;
        public override event Action ServerStopped;

        public override async Task<TcpClient> AcceptClientAsync(CancellationToken token)
        {
            var client = await _server.AcceptClientAsync(token);
            if (CurrentConnections >= MaxConnections)
            {
                var stream = client.GetStream();
                var message = "The server is overloaded, please try again later.";
                _ = SizeFirstReaderWriter.WriteAsync(stream, message, token);
                return null!;
            }
            return client;
        }
        public override Task<ClientObject> AddClientAsync(CancellationToken token)
        {
            CurrentConnections++;
            return _server.AddClientAsync(token);
        }
        public override void RemoveClient(ClientObject client)
        {
            _server.RemoveClient(client);
            CurrentConnections--;
        }
        public async override Task ListenAsync()
        {
            await _server.ListenAsync();
        }
        public override void Stop()
        {
            _server.Stop();
        }
    }
}
