using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace FWQ_Registry
{
    class Program
    {
        static void Main(string[] args)
        {
            string puertoEscucha;

            if (args.Length == 2)
            {
                puertoEscucha = args[1];
                Registry r = new Registry(puertoEscucha);
                r.Start();

            } else
            {
                Console.WriteLine("Introducir puerto de escucha");
            }
        }
    }
}
