using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpServer.Server;

namespace TcpServer.PCСomponents
{
    internal class PCComponentsServer : ServerObject
    {
        public PCComponentsServer(string ip, int port) : base(ip, port)
        {}
        protected override Task<ClientObject> AcceptClientAsync(CancellationToken token)
        {
            return base.AcceptClientAsync(token);
        }
    }
}
