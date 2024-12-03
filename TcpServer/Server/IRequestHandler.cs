using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer.Server
{
    public interface IRequestHandler
    {
        //Обрабатываем сообщение и возвращаем ответ
        public string Handler(string request);
    }
}
