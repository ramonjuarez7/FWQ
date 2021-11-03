using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace FWQ_WaitingTimeServer
{

    class TimeServer
    {
        //int numVisitantes;
        IPHostEntry host;
        IPAddress ipAddr;
        IPEndPoint endPoint;

        Socket s_Servidor;
        Socket s_Cliente;
        static int maximoPeticiones = 10;
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        
        public TimeServer(String puertoEscucha, String ipBroker, String puertoBroker)
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

        public static String Calculo(int numVisitantes)
        {
            StringBuilder sb = new StringBuilder();
            StreamReader sr = File.OpenText("atracciones.txt");
            String[] spliter;
            //String res = "";
            String line;
            int ciclo, visitantesCiclo, resultado = 0;
            while((line = sr.ReadLine()) != null)
            {
                spliter = line.Split(';');                
                ciclo = Int32.Parse(spliter[1]);
                visitantesCiclo = Int32.Parse(spliter[2]);
                resultado = (numVisitantes / visitantesCiclo) * ciclo;
                sb.Append(spliter[0] + ";" + resultado + ";\n");
            }
            sr.Close();
            return sb.ToString();
        }

        public void Start()
        {
            while (true)
            {
                allDone.Reset();
                Console.WriteLine("Esperando conexión...");
                s_Servidor.BeginAccept(new AsyncCallback(AcceptCallback), s_Servidor);
                allDone.WaitOne();
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read
                // more data.  
                content = state.sb.ToString();

                //numVisitantes será recibido por el broker
                int numVisitantes = 10;
                String resultado = Calculo(numVisitantes);
                Console.WriteLine("Enviando resultados..."); 
                // Echo the data back to the client.  
                Send(handler, resultado);    
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
