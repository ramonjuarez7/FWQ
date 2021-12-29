using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace FWQ_Visitor
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string ipBroker;
            string puertoBroker;
            string ipRegistry;
            string puertoRegistry;
            if(args.Length == 5)
            {
                ipBroker = args[1];
                puertoBroker = args[2];
                ipRegistry = args[3];
                puertoRegistry = args[4];

                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Inicio(ipBroker, puertoBroker, ipRegistry, puertoRegistry));
            } else
            {
                Console.WriteLine("Argumentos insuficientes");
            }
            
        }
    }
}
