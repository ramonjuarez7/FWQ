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
        {/*
            Socket listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket conexion;
            IPEndPoint connect = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6400);

            //conectamos el socket listen con la direccion ip y el puerto por el q escug¡ha 6400
            listen.Bind(connect);

            //si ya existen una conexión y alguien intenta conectarse, establece el número máximo de conexiones a la espera
            //y rechazará esa conexión
            listen.Listen(10);

            //el socket conexión va a ser igual a lo q retorne el método accept de socket listen
            conexion = listen.Accept();
            Console.WriteLine("Conexion aceptada: ");

            ///los sockets trabajan en bytes
            byte[] recibir_info = new byte[100];
            string data = "";
            int array_size = 0;

            //recibe la info por bytes en 4 parámetros(lo guardamos en,desde donde la guarda,hasta donde la guarda, 0)
            //si nos dan menos bytes de los que tenemos para q no queden espacios en vblanco lo igualamos a array_size
            array_size = conexion.Receive(recibir_info, 0, recibir_info.Length, 0);
            Array.Resize(ref recibir_info, array_size);
            data = Encoding.Default.GetString(recibir_info);

            Console.WriteLine("La info guardada es: {0}", data);

            //para q no se cierre el programa
            Console.ReadKey();

            //con todo esto el servidor está configurado para recibir info
            */

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
                //s.StartConsumingKafka();


            } else
            {
                Console.WriteLine("Los parámetros introducidos no son suficientes");
            }
            
        }
    }
}
