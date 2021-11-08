using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using Confluent.Kafka;

namespace FWQ_Engine
{
    class Program
    {
        static void Main(string[] args)
        {

            string ipBroker;
            string puertoBroker;
            string maxVisitantes;
            string ipTS;
            string puertoTS;

            if (args.Length == 6)
            {
                ipBroker = args[1];
                puertoBroker = args[2];
                maxVisitantes = args[3];
                ipTS = args[4];
                puertoTS = args[5];

                Console.WriteLine("Obtenidos datos necesarios.");

                Engine engine = new Engine(ipBroker, puertoBroker, maxVisitantes, ipTS, puertoTS);
                Thread th1 = new Thread(engine.SolicitudAccesoKafka);
                th1.Start();

                while (true)
                {
                  
                    engine = new Engine(ipBroker, puertoBroker, maxVisitantes, ipTS, puertoTS);
                    engine.StartTSConexion();
                    Thread.Sleep(5 * 1000);               
                    
                }

            }
            else
            {
                Console.WriteLine("Los parámetros introducidos deben ser 5.");
            }
            

        }
    }
}