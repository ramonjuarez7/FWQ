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
        {/*
            Socket listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
          
            //aqui la ip del el servidor al q se conecta
            IPEndPoint connect = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6400);

            listen.Connect(connect);

            //leemos linea de la consola y l aalmacenamos en data
            byte[] enviar_info = new byte[100];
            string data = "";
            Console.WriteLine("Ingrese la info a enviar");
            data = Console.ReadLine();

            enviar_info = Encoding.Default.GetBytes(data);

            listen.Send(enviar_info);
            Console.ReadKey();*/

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