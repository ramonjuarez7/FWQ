using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

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
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        static String path = Path.GetFullPath("..\\..\\..\\..\\usuarios.txt");
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

        static void lineChanger(string newText, string p, int lte)
        {
            string[] arrLine = File.ReadAllLines(p);
            arrLine[lte] = newText;
            File.WriteAllLines(p, arrLine);
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
                String[] c = content.Split(';');
                String mensaje = SelectOperation(c);               
                
                Console.WriteLine("Enviando resultados...");
                // Echo the data back to the client.  
                Send(handler, mensaje);
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

        public static String SelectOperation(String[] caso)
        {
            String mensaje = String.Empty;
            switch (caso[0])
            {
                case "Crear perfil":
                    mensaje = CrearPerfil(caso);
                    break;
                case "Editar perfil":
                    mensaje = EditarPerfil(caso);
                    break;
                case "Iniciar sesion":
                    mensaje = IniciarSesion(caso);
                    break;
                default:
                    
                    break;
            }
            return mensaje;
        }

        public static (int, String) BuscarUsuario(String alias, String path)
        {
            String password = String.Empty;
            int lineToEdit = -1;
            StreamReader sr = File.OpenText(path);
            String[] spliter;
            String line;
            bool existe = false;
            for (int i = 0; (line = sr.ReadLine()) != null && !existe; i++)
            {
                spliter = line.Split(';');
                if (spliter[0].Equals(alias))
                {
                    existe = true;
                    lineToEdit = i;
                    password = spliter[2];
                }
            }
            sr.Close();
            return (lineToEdit, password);
        }

        public static String CrearPerfil(String[] datos)
        {
            String mensaje = String.Empty, password = String.Empty;
            int lineToEdit = 0;

            (lineToEdit, password) = BuscarUsuario(datos[1], path);

            StreamWriter sw = File.AppendText(path);

            if (lineToEdit != -1)
            {
                mensaje = "El usuario ya existe!";
            } else
            {
                sw.WriteLine(datos[1] + ";" + datos[2] + ";" + datos[3] + ";");
                mensaje = "Usuario creado con exito.";
            }
            sw.Close();
            return mensaje;

            /*
            byte[] byteMensaje = Encoding.ASCII.GetBytes(mensaje);
            s_Cliente.Send(byteMensaje);
            sw.Close();

                case "Editar perfil":
                    sw.Close();
                    if (existe)
                    {
                        if (passwd.Equals(m[2]))
                        {
                        lineChanger(m[3] + ";" + m[4] + ";" + m[5] + ";", path, lineToEdit);
                        mensaje = "Cambios realizados con exito.";
                        } else
                        {
                            mensaje = "Password incorrecta.";
                        }
                            
                    } else
                    {
                        mensaje = "El usuario no existe!";
                    }
                    byte[] byteMensaje1 = Encoding.ASCII.GetBytes(mensaje);
                    s_Cliente.Send(byteMensaje1);
                    break;

               case "Entrar al parque":
                        if (existe)
                        {
                            if (passwd.Equals(m[3]))
                            {
                                mensaje = "Acceso concedido";
                            }
                            
                        } else
                        {
                            mensaje = "El usuario no existe!";
                        }
                        break;

               default:
                   break;
            }
            //sw.Close();
            //s_Cliente.Close();*/
        }

        public static String EditarPerfil(String[] datos)
        {
            String mensaje = String.Empty, password = String.Empty;
            int lineToEdit = 0;

            (lineToEdit, password) = BuscarUsuario(datos[1], path);
            if (lineToEdit != -1)
            {
                if (password.Equals(datos[2]))
                {
                    lineChanger(datos[3] + ";" + datos[4] + ";" + datos[5] + ";", path, lineToEdit);
                    mensaje = "Cambios realizados con exito.";
                }
                else
                {
                    mensaje = "Password incorrecta.";
                }

            }
            else
            {
                mensaje = "El usuario no existe!";
            }
            return mensaje;
        }

        public static String IniciarSesion(String[] datos)
        {
            String mensaje = String.Empty, password = String.Empty;
            int lineToEdit = 0;

            (lineToEdit, password) = BuscarUsuario(datos[1], path);
            if (lineToEdit != -1)
            {
                if (password.Equals(datos[2]))
                {
                    mensaje = "Credenciales correctas.";
                }
                else
                {
                    mensaje = "Password incorrecta.";
                }
            }
            else
            {
                mensaje = "El usuario no existe!";
            }
            return mensaje;
        }
    }
}
