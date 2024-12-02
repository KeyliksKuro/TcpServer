using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
    internal class ClientObject
    {
        private TcpClient _client;
        public TcpClient Client
        {
            get { return _client; }
        }
        public IRequestHandler RequestHandler { get; set; }

        private ServerObject _server;
        private NetworkStream _stream;

        public ClientObject(TcpClient client, IRequestHandler requestHandler, ServerObject server)
        {
            _client = client;
            _stream = client.GetStream();
            RequestHandler = requestHandler;
            _server = server;
        }
        public async Task ClientHandlerAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var request = await SizeFirstReaderWriter.ReadAsync(_stream, token);

                    //Обрабатываем сообщение и при необходимости отправляем ответ
                    var response = RequestHandler.Handler(request);
                    if (response != null)
                    {
                        _ = Task.Run(() => Write(response, token), token);
                    }
                }
            }
            catch (Exception) { throw; }
            finally { _server.RemoveClient(this); }
        }
        public async Task Write(string message, CancellationToken token)
        {
            await SizeFirstReaderWriter.WriteAsync(_stream, message, token);
        }
        public void Close() => Client.Close();
    }
}
