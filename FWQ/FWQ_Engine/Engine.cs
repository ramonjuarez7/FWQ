﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace FWQ_Engine
{
    class Engine
    {
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        private static String response = String.Empty;

        IPHostEntry hostBroker;
        IPHostEntry hostTimeServer;
        IPAddress ipAddrBroker;
        IPAddress ipAddrTimeServer;
        IPEndPoint endPointBroker;
        IPEndPoint endPointTimeServer;

        static Socket s_ClienteTS;

        static int numAtracciones = 5;

        public Engine(String ipBroker, String puertoBroker, String maximoVisitantes, String ipTimeServer, String puertoTimeServer)
        {
            hostTimeServer = Dns.GetHostEntry(ipTimeServer);
            ipAddrTimeServer = hostTimeServer.AddressList[0];
            int puerto = Int32.Parse(puertoTimeServer);
            endPointTimeServer = new IPEndPoint(ipAddrTimeServer, puerto);

            s_ClienteTS = new Socket(ipAddrTimeServer.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        }

        public void StartTSConexion()
        {
            // Connect to a remote device.  
            try
            {
                Console.CancelKeyPress += new ConsoleCancelEventHandler(StopClient);
                // Connect to the remote endpoint.  
                s_ClienteTS.BeginConnect(endPointTimeServer, new AsyncCallback(ConnectCallback), s_ClienteTS);
                connectDone.WaitOne();


                // Send test data to the remote device. 
                String enviar = "Solicitud de datos.";
                Send(s_ClienteTS, enviar);
                sendDone.WaitOne();

                // Receive the response from the remote device.  
                Receive(s_ClienteTS);
                receiveDone.WaitOne();

                // Write the response to the console.  
                Console.WriteLine("Response received : \n{0}", response);
                

                // Release the socket.  
                //s_ClienteTS.Shutdown(SocketShutdown.Both);
                //s_ClienteTS.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        protected static void StopClient(object sender, ConsoleCancelEventArgs args)
        {
            s_ClienteTS.Shutdown(SocketShutdown.Both);
            s_ClienteTS.Close();
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Receive(Socket client)
        {
            try
            {
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.  
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received.  
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Send(Socket client, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /*
        public void Send(String mensaje)
        {           
            byte[] byteMensaje = Encoding.ASCII.GetBytes(mensaje);
            s_ClienteTS.Send(byteMensaje);
            Console.WriteLine("Mensaje '" + mensaje + "' enviado");          
        }

        public int RecibirTS()
        {
            byte[] buffer = new byte[1024];
            s_ClienteTS.Receive(buffer);
            String mensaje = Encoding.ASCII.GetString(buffer);
            Console.WriteLine(mensaje);
            int res = Int32.Parse(mensaje);
            return res;
        }*/

    }
}