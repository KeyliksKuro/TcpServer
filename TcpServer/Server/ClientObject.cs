using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer.Server
{
    public class ClientObject
    {
        protected TcpClient _client;
        public TcpClient Client
        {
            get { return _client; }
        }
        public EndPoint EndPoint { get; }

        public IRequestHandler RequestHandler { get; set; }

        protected ServerObject _server;
        protected NetworkStream _stream;

        public ClientObject(TcpClient client, IRequestHandler requestHandler, ServerObject server)
        {
            _client = client;
            _stream = client.GetStream();
            RequestHandler = requestHandler;
            _server = server;
            EndPoint = client.Client.RemoteEndPoint;
        }
        public async Task ClientHandlerAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var request = await ReadAsync(token);

                    //Обрабатываем сообщение и при необходимости отправляем ответ
                    var response = RequestHandler.Handler(request);
                    if (response != null)
                    {
                        _ = Task.Run(() => WriteAsync(response, token), token);
                    }
                }
            }
            catch (Exception) { throw; }
            finally { _server.RemoveClient(this); }
        }
        public async Task<string> ReadAsync(CancellationToken token)
        {
            return await SizeFirstReaderWriter.ReadAsync(_stream, token);
        }
        public async Task WriteAsync(string message, CancellationToken token)
        {
            await SizeFirstReaderWriter.WriteAsync(_stream, message, token);
        }
        public void Close() => Client.Close();
    }
}
