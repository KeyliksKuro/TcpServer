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
    public abstract class TcpServerDecorator : ITcpServer
    {
        abstract public IPEndPoint EndPoint { get; }
        abstract public IRequestHandler? RequestHandler { get; set; }

        abstract public event Action ServerStarted;
        abstract public event Action<ClientObject> ClientAdded;
        abstract public event Action<ClientObject> ClientDisconnected;
        abstract public event Action ServerStopped;

        protected ServerObject _server;
        public TcpServerDecorator(ServerObject server)
        {
            _server = server;
        }

        abstract public Task<TcpClient> AcceptClientAsync(CancellationToken token);
        abstract public Task<ClientObject> AddClientAsync(CancellationToken token);
        abstract public Task ListenAsync();
        abstract public void RemoveClient(ClientObject client);
        abstract public void Stop();
    }
}
