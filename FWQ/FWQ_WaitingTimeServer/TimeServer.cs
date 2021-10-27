using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;

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

        public String Calculo(String atraccion, int numVisitantes)
        {
            StreamReader sr = File.OpenText("bbdd.txt");
            String[] spliter;
            String res;
            int ciclo, visitantesCiclo, resultado;
            foreach(string line in sr.ReadLine())
            {
                spliter = line.Split_(';');
                if (spliter[0].Equals(atraccion))
                {
                    ciclo = Int32.Parse(spliter[1]);
                    visitantesCiclo = Int32.Parse(spliter[2]);
                    resultado = (numVisitantes / visitantesCiclo) * ciclo;
                    res = "" + resultado;
                }
            }
            sr.Close();
            return res;
        }

        public void Start()
        {
            byte[] buffer = new byte[1024];
            string mensaje;
            int numVisitantes = 20; //Este numero lo recogerá el sensor

            //acepta la conexion
            s_Cliente = s_Servidor.Accept();
        
            s_Cliente.Receive(buffer);
            mensaje = Encoding.ASCII.GetString(buffer);
            String resultado =  Calculo(mensaje, numVisitantes);
            byte[] byteMensaje = Encoding.ASCII.GetBytes(resultado);
            Console.WriteLine("Calculado el Tiempo de Espera en atracción " + mensaje + ", enviando resultado = " + resultado);
            s_Cliente.Send(byteMensaje);
        }
    }
}
