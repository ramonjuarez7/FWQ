﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FWQ_Engine
{
    class Engine
    {
        IPHostEntry hostBroker;
        IPHostEntry hostTimeServer;
        IPAddress ipAddrBroker;
        IPAddress ipAddrTimeServer;
        IPEndPoint endPointBroker;
        IPEndPoint endPointTimeServer;


        Socket s_ClienteBroker;
        Socket s_ClienteTS;

        public Engine(String ipBroker, String puertoBroker, String maximoVisitantes, String ipTimeServer, String puertoTimeServer)
        {
            hostTimeServer = Dns.GetHostEntry(ipTimeServer);
            ipAddrTimeServer = hostTimeServer.AddressList[0];
            int puerto = Int32.Parse(puertoTimeServer);
            endPointTimeServer = new IPEndPoint(ipAddrTimeServer, puerto);

            hostBroker = Dns.GetHostEntry(ipBroker);
            ipAddrBroker = hostBroker.AddressList[0];
            int puerto2 = Int32.Parse(puertoBroker);
            endPointTimeServer = new IPEndPoint(ipAddrBroker, puerto2);


            //(para escuchar desde esa adress familia, tipo de socket q usamos, protocolo por el q envia y recibe info )
            s_ClienteTS = new Socket(ipAddrTimeServer.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            //s_ClienteBroker = new Socket(ipAddrBroker.AddressFamily, SocketType.Stream, ProtocolType.Tcp);




        }

        /*
        public void StartBrokerConexion()
        {
            s_ClienteBroker.Connect(endPointBroker);

        }*/

        public void StartTSConexion()
        {
            s_ClienteTS.Connect(endPointTimeServer);

        }

        public void StopTSConexion()
        {
            s_ClienteTS.Close();
        }

        public void Send(String mensaje)
        {
            
            byte[] byteMensaje = Encoding.ASCII.GetBytes(mensaje);
            s_ClienteTS.Send(byteMensaje);
            Console.WriteLine("Mensaje enviado");
            
        }

        public int RecibirTS()
        {
            byte[] buffer = new byte[1024];
            s_ClienteTS.Receive(buffer);
            String mensaje = Encoding.ASCII.GetString(buffer);
            int res = Int32.Parse(mensaje);
            return res;
        }

    }
}