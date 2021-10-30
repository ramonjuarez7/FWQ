using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;

namespace FWQ_Registry
{
    class Registry
    {
        IPHostEntry host;
        IPAddress ipAddr;
        IPEndPoint endPoint;

        Socket s_Servidor;
        Socket s_Cliente;
        static int maximoPeticiones = 10;
        public Registry(String puertoEscucha)
        {
            host = Dns.GetHostEntry("localhost");
            ipAddr = host.AddressList[0];
            int puerto = Int32.Parse(puertoEscucha);
            endPoint = new IPEndPoint(ipAddr, puerto);


            //(para escuchar desde esa adress familia, tipo de socket q usamos, protocolo por el q envia y recibe info )
            s_Servidor = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //desde donde va a escuchar
            s_Servidor.Bind(endPoint);

            //nº max de conexiones q va a tener en cola antes de rechazar
            s_Servidor.Listen(maximoPeticiones);

            Console.WriteLine("Escuchando al puerto " + puertoEscucha);
        }

        public void Start()
        {
            byte[] buffer = new byte[1024];
            string mensaje;

            //acepta la conexion
            s_Cliente = s_Servidor.Accept();

            while (true)
            {
                s_Cliente.Receive(buffer);
                mensaje = Encoding.ASCII.GetString(buffer);
                String[] m = mensaje.Split(";");
                //
                switch (m[0])
                {
                    case "Crear perfil":
                        
                        break;

                    case "Editar perfil":

                        break;

                    case "Entrar al parque":

                        break;

                    default:
                        break;
                }
            }
        }
    }
}
