using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using Confluent.Kafka;
using System.IO;

namespace FWQ_Engine
{
    class Engine
    {
        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        private static String response = String.Empty;

        IPHostEntry hostTimeServer;
        IPAddress ipAddrTimeServer;
        IPEndPoint endPointTimeServer;

        int maxVisitantes;
        int visitantesActuales;

        static Socket s_ClienteTS;
        static ProducerConfig pconfig;
        static ConsumerConfig cconfig;
        static String path = Path.GetFullPath("..\\..\\..\\..\\mapa.txt");

        static int numAtracciones = 5;
        
        String[,] mapaData;

        public Engine(String ipBroker, String puertoBroker, String maximoVisitantes, String ipTimeServer, String puertoTimeServer)
        {
            //hostTimeServer = Dns.GetHostEntry(ipTimeServer);
            //ipAddrTimeServer = hostTimeServer.AddressList[0];
            int puerto = Int32.Parse(puertoTimeServer);
            endPointTimeServer = new IPEndPoint(IPAddress.Parse(ipTimeServer), puerto);

            s_ClienteTS = new Socket(endPointTimeServer.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            maxVisitantes = Int32.Parse(maximoVisitantes);
            visitantesActuales = 0;

            pconfig = new ProducerConfig
            {
                BootstrapServers = ipBroker + ":" + puertoBroker,
                SecurityProtocol = SecurityProtocol.Plaintext
            };

            cconfig = new ConsumerConfig
            {
                BootstrapServers = ipBroker + ":" + puertoBroker,
                SecurityProtocol = SecurityProtocol.Plaintext,
                GroupId = "my-group2"
            };

            StreamReader leer = new StreamReader(path);
            mapaData = new String[numAtracciones,4];
            String cadena;
            for (int i = 0; (cadena = leer.ReadLine()) != null; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    mapaData[i, j] = cadena.Split(';')[j];
                }               
            }
            leer.Close();

        }

        public void EnviarAforoKafka()
        {
            String enviar = visitantesActuales + ":" + maxVisitantes + ":";
            using (var producer = new ProducerBuilder<Null, string>(pconfig).Build())
            {
                var dr = producer.ProduceAsync("visitantes2", new Message<Null, string> { Value = enviar }).Result;
                Console.WriteLine($"Delivered '{dr.Value}' to: {dr.TopicPartitionOffset}");
            }       
        }

        public String[,] ConstruirMapa()
        {
            String[,] mapa = {

                { "# ", "# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                { "# ", ". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ",". ","#"},
                {"# ", "# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","# ","#" }

            };

            int x;
            int y;
            String tiempo;

            for (int i = 0; i < numAtracciones; i++)
            {
                x = Int32.Parse(mapaData[i, 1].Split(':')[0]);
                y = Int32.Parse(mapaData[i, 1].Split(':')[1]);
                tiempo = mapaData[i, 2];
                if (mapa[x, y].Equals(". "))
                {
                    mapa[x, y] = tiempo;
                    if(Int32.Parse(tiempo) < 10)
                    {
                        mapa[x, y] += " ";
                    }
                } 
            }
            return mapa;
        }            
        
        public String ConstruyeStringMapa(String[,] mapa)
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < 22; i++)
            {
                for(int j = 0; j < 22; j++)
                {
                    sb.Append(mapa[i, j]);
                }
                sb.Append('\n');
            }
            //Console.WriteLine(sb.ToString());
            return sb.ToString();
        }

        public void EnviarMapaKafka()
        {
            String[,] mapa = ConstruirMapa();
            String mapaString = ConstruyeStringMapa(mapa);
            using (var producer = new ProducerBuilder<Null, string>(pconfig).Build())
            {
                var dr = producer.ProduceAsync("sd-events", new Message<Null, string> { Value = mapaString }).Result;
                Console.WriteLine($"Delivered \n'{dr.Value}' to: {dr.TopicPartitionOffset}");
            }

        }

        public void AlmacenarTiemposDeEspera(String res)
        {
            String[] lineas = res.Split('\n');
            String[] datos;
            for(int i = 0; i < lineas.Length; i++)
            {
                datos = lineas[i].Split(';');
                for(int j = 0; j < numAtracciones; j++)
                {
                    if (mapaData[j, 0].Equals(datos[0]))
                    {
                        mapaData[j, 2] = datos[1];
                    }
                }
            }
        }
        
        public void SolicitudParqueKafka()
        {
            using (var consumer = new ConsumerBuilder<Null, string>(cconfig).Build())
            {
                consumer.Subscribe("visitantes4");
                try
                {
                    while (true)
                    {
                        var consumeResult = consumer.Consume();
                        if (consumeResult.Message.Value.Equals("Mapa"))
                        {
                            EnviarMapaKafka();
                        }
                        Console.WriteLine("Enviado mapa:");
                        Console.WriteLine(consumeResult.Message.Value);
                    }
                }
                catch (Exception)
                {
                    consumer.Close();
                }
            }
        }

        public void SolicitudAccesoKafka()
        {
            using (var consumer = new ConsumerBuilder<Null, string>(cconfig).Build())
            {
                consumer.Subscribe("visitantes");
                try
                {
                    while (true)
                    {
                        Console.WriteLine("Visitantes actuales: " + visitantesActuales);
                        var consumeResult = consumer.Consume();
                        switch(consumeResult.Message.Value){
                            case "Acceso":
                            this.EnviarAforoKafka();
                                if(visitantesActuales < maxVisitantes)
                                {
                                    visitantesActuales++;
                                    Console.WriteLine("Visitantes actuales: " + visitantesActuales);
                                }
                                break;
                            case "Salgo":
                                visitantesActuales--;
                                Console.WriteLine("Visitantes actuales: " + visitantesActuales);
                                break;
                            case "Mapa":
                                EnviarMapaKafka();
                                break;
                            default:
                                break;
                        }
                        Console.WriteLine("Enviados visitantes actuales:");
                        Console.WriteLine(consumeResult.Message.Value);
                    }
                }
                catch (Exception)
                {
                    consumer.Close();
                }
            }
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
                AlmacenarTiemposDeEspera(response);
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
        
        //Conecta con el servidor de OpenWeather
        public void conectarOpenWeatherAsync()
        {

            //OTRA FORMA DE GHACER EL CÓDIGO
            /* string url = @"http://https://api.openweathermap.org/data/2.5/weather?id=2521978&appid=573dad31e053e6563583f75c34a695b9";
             WebRequest webRequest = WebRequest.Create(url);
             HttpWebResponse httpwebResponse = null;
             httpwebResponse = (HttpWebResponse)webRequest.GetResponse();

             string result = string.Empty;

             using (Stream stream = httpwebResponse.GetResponseStream())
             {
                 StreamReader streamReader = new StreamReader(stream);
                 result = streamReader.ReadToEnd();
                 streamReader.Close();
             }
            */


            // creo un array donde almaceno diferentes ciudades
            
            string[] ciudades = {"Alicante", "Barcelona", "Helsinki", "Paris", "Milan", "Murcia", "Cáceres" };

            var seed = Environment.TickCount;
            var random = new Random(seed);

            //con una variable aleatoria, saco la posicion de 4 ciudades y las almaceno
            int value = random.Next(0, 8);
            int c1 = value;
            int c2 = value;
            int c3 = value;
            int c4 = value;
            

            //almaceno la url con los datos de una ciudad
            string url1 = "http://api.openweathermap.org/data/2.5/weather?q=" + ciudades[c1] + "&appid=bc76a4f700dcec5a1977869940c499cb";
            string url2 = "http://api.openweathermap.org/data/2.5/weather?q=" + ciudades[c2] + "&appid=8a0609a61e66b01bb4d3f0d2e27da57a";
            string url3 = "http://api://.openweathermap.org/data/2.5/weather?q=" + ciudades[c2] + "&appid=1f5660acf5f16819b8294daf04e569d6";
            string url4 = "http://api://.openweathermap.org/data/2.5/weather?q=" + ciudades[c2] + "&appid=0d142b606d67e84346cbd2627b783d72";
                
            //deserializo los archivos json de cada url
            var response = new WebClient().DownloadString(url1);
            dynamic posts = JsonConvert.DeserializeObject(response);

            var response2 = new WebClient().DownloadString(url2);
            dynamic posts2 = JsonConvert.DeserializeObject(response2);

            var response3 = new WebClient().DownloadString(url3);
            dynamic posts3 = JsonConvert.DeserializeObject(response3);

            var response4 = new WebClient().DownloadString(url4);
            dynamic posts4 = JsonConvert.DeserializeObject(response4);


            //recorro cada url obteniendo los datos de la temperatura
            foreach (var post in posts)
            {
                    Console.WriteLine("Nombre ciudad: " + post.name ); //=> Nombre ciudad Alicante
                    
                    //si la temperatura no es la correcta se cierra esa zona del parque
                    if (float.Parse(post.main.temp, CultureInfo.InvariantCulture.NumberFormat)  < 293.15 )
                    {
                        Console.WriteLine("Temperatura menor a la permitida, se cierra esta zona del parque " + ciudades[c1]);
                    }
                    else
                    {
                        if (float.Parse(post.main.temp, CultureInfo.InvariantCulture.NumberFormat) > 303.15)
                        {
                            Console.WriteLine("Temperatura mayor a la permitida, se cierra esta zona del parque " + ciudades[c1]);
                        }
                    }
            }

            foreach (var post2 in posts2)
            {
                Console.WriteLine("Nombre ciudad: " + post2.name); //=> Nombre ciudad Alicante

                if (float.Parse(post2.main.temp, CultureInfo.InvariantCulture.NumberFormat) < 293.15)
                {
                    Console.WriteLine("Temperatura menor a la permitida, se cierra esta zona del parque " + ciudades[c2]);
                }
                else
                {
                    if (float.Parse(post2.main.temp, CultureInfo.InvariantCulture.NumberFormat) > 303.15)
                    {
                        Console.WriteLine("Temperatura mayor a la permitida, se cierra esta zona del parque " + ciudades[c2]);
                    }
                }
            }

            foreach (var post3 in posts3)
            {
                Console.WriteLine("Nombre ciudad: " + post3.name); //=> Nombre ciudad Alicante

                if (float.Parse(post3.main.temp, CultureInfo.InvariantCulture.NumberFormat) < 293.15)
                {
                    Console.WriteLine("Temperatura menor a la permitida, se cierra esta zona del parque " + ciudades[c3]);
                }
                else
                {
                    if (float.Parse(post3.main.temp, CultureInfo.InvariantCulture.NumberFormat) > 303.15)
                    {
                        Console.WriteLine("Temperatura mayor a la permitida, se cierra esta zona del parque " + ciudades[c3]);
                    }
                }
            }

            foreach (var post4 in posts)
            {
                Console.WriteLine("Nombre ciudad: " + post4.name); //=> Nombre ciudad Alicante

                if (float.Parse(post4.main.temp, CultureInfo.InvariantCulture.NumberFormat) < 293.15)
                {
                    Console.WriteLine("Temperatura menor a la permitida, se cierra esta zona del parque " + ciudades[c4]);
                }
                else
                {
                    if (float.Parse(post4.main.temp, CultureInfo.InvariantCulture.NumberFormat) > 303.15)
                    {
                        Console.WriteLine("Temperatura mayor a la permitida, se cierra esta zona del parque de " + ciudades[c4]);
                    }
                }
            }

            
        }

    }
}
