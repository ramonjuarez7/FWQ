using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FWQ_Visitor
{
    public partial class EditarPerfil : Form
    {
        String ipBroker;
        String puertoBroker;
        String ipRegistry;
        String puertoRegistry;

        public EditarPerfil(string ipB, string puertoB, string ipR, string puertoR)
        {
            ipBroker = ipB;
            puertoBroker = puertoB;
            ipRegistry = ipR;
            puertoRegistry = puertoR;
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Visitor visitor = new Visitor(ipBroker, puertoBroker, ipRegistry, puertoRegistry);
            String[] mensaje = new string[6];
            mensaje[0] = "Editar perfil";
            mensaje[1] = textBox5.Text;
            mensaje[2] = textBox4.Text;
            mensaje[3] = textBox1.Text;
            mensaje[4] = textBox2.Text;
            mensaje[5] = textBox3.Text;
            visitor.StartRConexion();
            visitor.SendInfoForUpdate(mensaje);
            label7.Text = visitor.RecibirR();
            visitor.StopRConexion();
        }
    }
}
