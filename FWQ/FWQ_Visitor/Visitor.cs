using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FWQ_Visitor
{
    class Visitor
    {

        IPHostEntry hostRegistry;
        IPAddress ipAddrRegistry;
        IPEndPoint endPointRegistry;

        Socket s_ClienteR;

        public Visitor(String ipBroker, String puertoBroker, String ipRegistry, String puertoRegistry)
        {
            hostRegistry = Dns.GetHostEntry(ipRegistry);
            ipAddrRegistry = hostRegistry.AddressList[0];
            int puerto = Int32.Parse(puertoRegistry);
            endPointRegistry = new IPEndPoint(ipAddrRegistry, puerto);

            //(para escuchar desde esa adress familia, tipo de socket q usamos, protocolo por el q envia y recibe info )
            s_ClienteR = new Socket(ipAddrRegistry.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //s_ClienteBroker = new Socket(ipAddrBroker.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public void StartRConexion()
        {
            s_ClienteR.Connect(endPointRegistry);
            //endPointRegistry = new IPEndPoint(ipAddrRegistry, endPointRegistry.Port);

        }

        public void StopRConexion()
        {
            s_ClienteR.Close();
        }

        public void SendFullData(String[] mensaje)
        {
            byte[] byteMensaje = Encoding.ASCII.GetBytes(mensaje[0] + ";" + mensaje[1] + ";" + mensaje[2] + ";" + mensaje[3] + ";");
            s_ClienteR.Send(byteMensaje);
            Console.WriteLine("Mensaje enviado");
        }

        public void SendInfoForUpdate(String[] mensaje)
        {
            byte[] byteMensaje = Encoding.ASCII.GetBytes(mensaje[0] + ";" + mensaje[1] + ";" + mensaje[2] + ";" + mensaje[3] + ";" + mensaje[4] + ";" + mensaje[5] + ";");
            s_ClienteR.Send(byteMensaje);
            Console.WriteLine("Mensaje enviado");
        }

        public void SendMessage(String mensaje)
        {
            byte[] byteMensaje = Encoding.ASCII.GetBytes(mensaje);
            s_ClienteR.Send(byteMensaje);
            Console.WriteLine("Mensaje enviado");
        }

        public void SendUserPasswd(String[] mensaje)
        {
            byte[] byteMensaje = Encoding.ASCII.GetBytes(mensaje[0] + ";" + mensaje[1] + ";" + mensaje[3] + ";");
            s_ClienteR.Send(byteMensaje);
            Console.WriteLine("Mensaje enviado");
        }

        public String RecibirR()
        {
            byte[] buffer = new byte[1024];
            s_ClienteR.Receive(buffer);
            String mensaje = Encoding.ASCII.GetString(buffer);
            return mensaje;
        }
    }
}
