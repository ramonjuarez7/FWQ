using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FWQ_Visitor
{
    public partial class IniciarSesion : Form
    {
        String ipBroker;
        String puertoBroker;
        String ipRegistry;
        String puertoRegistry;
        public IniciarSesion(string ipB, string puertoB, string ipR, string puertoR)
        {
            ipBroker = ipB;
            puertoBroker = puertoB;
            ipRegistry = ipR;
            puertoRegistry = puertoR;
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            String llamador = "Iniciar sesion";
            String[] mensaje = new string[2];
            mensaje[0] = textBox1.Text;
            mensaje[1] = textBox2.Text;
            Visitor visitor = new Visitor(ipBroker, puertoBroker, ipRegistry, puertoRegistry, llamador, mensaje);
            label3.Text = visitor.StartRConexion();
            if(label3.Text.Equals("Credenciales correctas."))
            {
                Thread.Sleep(1 * 1000);
                InteriorParque ip = new InteriorParque();
                ip.Show();
            }
        }
    }
}
