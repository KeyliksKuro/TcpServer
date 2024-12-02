using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
    internal class Client
    {
        Socket _client; // подключенный клиент
        HTTPHeaders Headers; // распарсенные заголовки

        public Client(Socket socket)
        {
            _client = socket;
            byte[] data = new byte[_client.ReceiveBufferSize]; // _client.ReceiveBufferSize - хранит значение полученных данных
            string request = string.Empty;
            _client.Receive(data); // считываем входящий запрос и записываем его в наш буфер data
            request = Encoding.UTF8.GetString(data); // преобразуем принятые нами байты с помощью кодировки UTF8 в читабельный вид
            //Чё за нах?
            if (request == string.Empty)
            {
                _client.Close();
                return;
            }
            Headers = HTTPHeaders.Parse(request);
            Console.WriteLine($"[{_client.RemoteEndPoint}]\nFile: {Headers.File}\nDate: {DateTime.Now}");
            if (Headers.RealPath.IndexOf("..") != -1)
            {
                SendError(404);
                _client.Close();
                return;
            }
            if (File.Exists(Headers.RealPath))
            {
                GetSheet();
            }
            else
            {
                SendError(404);
            }
            _client.Close();
        }
        public void SendError(int code)
        {
            string html = $"<html><head><title></title></head><body><h1>Error {code}</h1></body></html>";
            string headers = $"HTTP/1.1 {code} OK\nContent-type: text/html\nContent-Length: {html.Length}\n\n{html}";
            byte[] data = Encoding.UTF8.GetBytes(headers);
            _client.Send(data, data.Length, SocketFlags.None);
            _client.Close();
        }
    }
}
