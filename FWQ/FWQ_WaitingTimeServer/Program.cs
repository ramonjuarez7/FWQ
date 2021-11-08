using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace FWQ_WaitingTimeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string ipBroker;
            string puertoBroker;
            string puertoEscucha;

            if (args.Length == 4)
            {
                puertoEscucha = args[1];
                ipBroker = args[2];
                puertoBroker = args[3];

                TimeServer s = new TimeServer(puertoEscucha, ipBroker, puertoBroker);
                Thread th1 = new Thread(s.Start);
                th1.Start();
                s.StartConsumingKafka();


            } else
            {
                Console.WriteLine("Los parámetros introducidos no son suficientes");
            }
            
        }
    }
}
