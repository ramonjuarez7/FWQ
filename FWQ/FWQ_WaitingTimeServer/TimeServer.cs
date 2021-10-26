using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FWQ_WaitingTimeServer
{
    class TimeServer
    {
        IPHostEntry host;
        IPAddress ipAddr;
        IPEndPoint endPoint;

        Socket s_Servidor;
        Socket s_Cliente;
        static int maximoPeticiones = 10;
        public TimeServer(String puertoEscucha, String ipBroker, String puertoBroker)
        {
            host = Dns.GetHostByName("localhost");
            ipAddr = host.AddressList[0];
            int puerto = Int32.Parse(puertoEscucha);
            endPoint = new IPEndPoint(ipAddr, puerto);


            //(para escuchar desde esa adress familia, tipo de socket q usamos, protocolo por el q envia y recibe info )
            s_Servidor = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //desde donde va a escuchar
            s_Servidor.Bind(endPoint);

            //nº max de conexiones q va a tener en cola antes de rechazar
            s_Servidor.Listen(maximoPeticiones);


        }

        public void Start()
        {
            byte[] buffer = new byte[1024];
            string mensaje;

            //acepta la conexion
            s_Cliente = s_Servidor.Accept();


            s_Cliente.Receive(buffer);
            mensaje = Encoding.ASCII.GetString(buffer);
            Console.WriteLine("Se recibió el mensaje: " + mensaje);
        }
    }
}
