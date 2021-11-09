using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using Confluent.Kafka;
using Microsoft;
using Microsoft.AspNetCore.Hosting;

namespace FWQ_Visitor
{
    class Visitor
    {
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        private static String response = String.Empty;
        private static String llamador;
        IPHostEntry hostRegistry;
        IPAddress ipAddrRegistry;
        IPEndPoint endPointRegistry;
        static String[] mensaje;
        static Socket s_ClienteR;
        ProducerConfig pconfig;
        ConsumerConfig cconfig;


        public Visitor(String ipBroker, String puertoBroker, String ipRegistry, String puertoRegistry, String ll, String[] m)
        {
            mensaje = m;
            llamador = ll;
            //hostRegistry = Dns.GetHostEntry(ipRegistry);
            //ipAddrRegistry = hostRegistry.AddressList[0];
            int puerto = Int32.Parse(puertoRegistry);
            endPointRegistry = new IPEndPoint(IPAddress.Parse(ipRegistry), puerto);

            //(para escuchar desde esa adress familia, tipo de socket q usamos, protocolo por el q envia y recibe info )
            s_ClienteR = new Socket(endPointRegistry.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //s_ClienteBroker = new Socket(ipAddrBroker.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            pconfig = new ProducerConfig
            {
                BootstrapServers = ipBroker + ":" + puertoBroker,
                SecurityProtocol = SecurityProtocol.Plaintext
            };

            cconfig = new ConsumerConfig
            {
                BootstrapServers = ipBroker + ":" + puertoBroker,
                SecurityProtocol = SecurityProtocol.Plaintext,
                GroupId = "my-group3"
            };
        }

        public bool RecibirAforoKafka()
        {
            bool resultado = false;
            using (var consumer = new ConsumerBuilder<Null, string>(cconfig).Build())
            {
                consumer.Subscribe("visitantes2");
                try
                {
                    var consumeResult = consumer.Consume();
                    String[] recibido = consumeResult.Message.Value.Split(':');
                    if (Int32.Parse(recibido[0]) == Int32.Parse(recibido[1]))
                    {
                        resultado = false;
                    }
                    else
                    {
                        resultado = true;
                    }
                }
                catch (Exception)
                {
                    consumer.Close();
                }
            }
            return resultado;
        }

        public void SolicitudAccesoKafka()
        {
            using (var producer = new ProducerBuilder<Null, string>(pconfig).Build())
            {
                var dr = producer.ProduceAsync("visitantes", new Message<Null, string> { Value = "Acceso" }).Result;
                Console.WriteLine($"Delivered '{dr.Value}' to: {dr.TopicPartitionOffset}");
            }
        }
        public void SalirParqueKafka()
        {
            using (var producer = new ProducerBuilder<Null, string>(pconfig).Build())
            {
                var dr = producer.ProduceAsync("visitantes", new Message<Null, string> { Value = "Salgo" }).Result;
                Console.WriteLine($"Delivered '{dr.Value}' to: {dr.TopicPartitionOffset}");
            }
        }

        public void SolicitarMapaKafka()
        {
            using (var producer = new ProducerBuilder<Null, string>(pconfig).Build())
            {
                //while (true)
                //{
                    var dr = producer.ProduceAsync("visitantes", new Message<Null, string> { Value = "Mapa" }).Result;
                    Console.WriteLine($"Delivered '{dr.Value}' to: {dr.TopicPartitionOffset}");
                    //RecibirMapaKafka();
                    Thread.Sleep(1 * 1000);
                //}
                
            }
        }
        public String RecibirMapaKafka()
        {
            String mapa = String.Empty;
            using (var consumer = new ConsumerBuilder<Null, string>(cconfig).Build())
            {
                consumer.Subscribe("sd-events");
                try
                {
                    //no consume correctamente
                    var consumeResult = consumer.Consume();
                    mapa = consumeResult.Message.Value;
                    return mapa;
                }
                catch (Exception)
                {
                    consumer.Close();
                }
            }
            return mapa;
        }

        public static String CreaMensajeLlamada(String ll, String[] m)
        {
            StringBuilder mensaje = new StringBuilder();
            switch (ll)
            {
                case "Crear perfil":
                    mensaje.Append(ll + ";" + m[0] + ";" + m[1] + ";" + m[2] + ";");
                    break;
                case "Editar perfil":
                    mensaje.Append(ll + ";" + m[0] + ";" + m[1] + ";" + m[2] + ";" + m[3] + ";" + m[4] + ";");
                    break;
                case "Iniciar sesion":
                    mensaje.Append(ll + ";" + m[0] + ";" + m[1] + ";");
                    break;
                default:
                    break;
            }
            return mensaje.ToString();
        }

        public String StartRConexion()
        {
            // Connect to a remote device.  
            try
            {
                Console.CancelKeyPress += new ConsoleCancelEventHandler(StopClient);
                // Connect to the remote endpoint.  
                s_ClienteR.BeginConnect(endPointRegistry, new AsyncCallback(ConnectCallback), s_ClienteR);
                connectDone.WaitOne();


                // Send test data to the remote device. 
                
                String enviar = CreaMensajeLlamada(llamador,mensaje);
                Send(s_ClienteR, enviar);
                sendDone.WaitOne();

                // Receive the response from the remote device.  
                Receive(s_ClienteR);
                receiveDone.WaitOne();

                // Write the response to the console.  
                return response;


                // Release the socket.  
                //s_ClienteR.Shutdown(SocketShutdown.Both);
                //s_ClienteR.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return "";

        }

        protected static void StopClient(object sender, ConsoleCancelEventArgs args)
        {
            s_ClienteR.Shutdown(SocketShutdown.Both);
            s_ClienteR.Close();
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
    }
}
