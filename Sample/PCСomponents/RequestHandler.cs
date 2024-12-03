using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpServer.Server;

namespace Sample.PCСomponents
{
    internal class RequestHandler : IRequestHandler
    {
        public string Handler(string request)
        {
            Console.WriteLine("Получен запрос на получение цены комплектующей.");
            Console.WriteLine("Цена отправлена.");
            return "Цена комплюктующей.";
        }
    }
}
