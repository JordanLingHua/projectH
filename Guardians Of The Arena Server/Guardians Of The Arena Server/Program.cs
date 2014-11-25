using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Guardians_Of_The_Arena_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            //server.Listen();
            server.listenerThread.Start();
            server.loopThread.Start();
        }
    }
}
