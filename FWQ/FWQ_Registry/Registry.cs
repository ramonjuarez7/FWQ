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

        static void lineChanger(string newText, string p, int lte)
        {
            string[] arrLine = File.ReadAllLines(p);
            arrLine[lte - 1] = newText;
            File.WriteAllLines(p, arrLine);
        }

        public void Start()
        {
            byte[] buffer = new byte[1024];
            string mensaje;

            //acepta la conexion
            s_Cliente = s_Servidor.Accept();

            s_Cliente.Receive(buffer);
            mensaje = Encoding.ASCII.GetString(buffer);
            String[] m = mensaje.Split(";");
                //
            String path = Path.GetFullPath("..\\..\\..\\..\\usuarios.txt");
            StreamReader sr = File.OpenText(path);
            String[] spliter;
                //String res = "";
            String line, passwd = "";
            int lineToEdit = 0;
            bool existe = false;
            for(int i = 0; (line = sr.ReadLine()) != null && !existe; i++)
            {
                spliter = line.Split(';');
                if (spliter[0].Equals(m[1]))
                {
                        existe = true;
                        lineToEdit = i;
                        passwd = spliter[2];
                }
            }
            sr.Close();

            StreamWriter sw = File.AppendText(path);

            switch (m[0])
            {
                case "Crear perfil":
                    if (existe)
                    {
                        mensaje = "El usuario ya existe!";
                    } else
                    {
                        sw.WriteLine(m[1] + ";" + m[2] + ";" + m[3] + ";");
                         mensaje = "Usuario creado con exito.";

                    }
                    byte[] byteMensaje = Encoding.ASCII.GetBytes(mensaje);
                    s_Cliente.Send(byteMensaje);
                    break;

                case "Editar perfil":
                    if (existe)
                    {
                        if (passwd.Equals(m[3]))
                        {
                        lineChanger(m[3] + ";" + m[4] + ";" + m[5] + ";", path, lineToEdit);
                        mensaje = "Cambios realizados con éxito.";
                        } else
                        {
                            mensaje = "Contraseña incorrecta.";
                        }
                            
                    } else
                    {
                        mensaje = "El usuario no existe!";
                    }
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
            sw.Close();
            //s_Cliente.Close();
        }
    }
}
