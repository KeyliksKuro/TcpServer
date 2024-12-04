using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer.Server
{
    public interface ITcpServer
    {
        public IPEndPoint EndPoint { get; }
        public IRequestHandler? RequestHandler { get; set; }

        public event Action ServerStarted;
        public event Action<ClientObject> ClientAdded;
        public event Action<ClientObject> ClientDisconnected;
        public event Action ServerStopped;
        public Task ListenAsync();
        public Task<TcpClient> AcceptClientAsync(CancellationToken token);
        public Task<ClientObject> AddClientAsync(CancellationToken token);
        public void RemoveClient(ClientObject client);
        public void Stop();
    }
}
