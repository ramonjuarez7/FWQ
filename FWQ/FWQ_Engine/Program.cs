using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

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
            int maximoAtracciones = 5;
            int segundosEspera = 10;
            DateTime tiempoActual;
            DateTime tiempoNuevo = DateTime.Now;
            while (true)
            {
                tiempoActual = DateTime.Now;
                TimeSpan ts = tiempoNuevo - tiempoActual;
                if (ts.TotalSeconds > segundosEspera) { 
                    for(int i = 1; i <= maximoAtracciones; i++)
                    {

                    }
                }
            }

            Engine c = new Engine("localhost", 4404);
            c.Start();
            c.Send("Hola soy cliente");
            Console.ReadKey();

        }
    }
}