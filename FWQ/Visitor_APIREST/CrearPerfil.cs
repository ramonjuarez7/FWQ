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
    public partial class CrearPerfil : Form
    {
        String ipBroker;
        String puertoBroker;
        String ipRegistry;
        String puertoRegistry;
            
        public CrearPerfil(string ipB, string puertoB, string ipR, string puertoR)
        {
            ipBroker = ipB;
            puertoBroker = puertoB;
            ipRegistry = ipR;
            puertoRegistry = puertoR;
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String llamador = "Crear perfil";           
            String[] mensaje = new string[3];
            mensaje[0] = textBox1.Text;
            mensaje[1] = textBox2.Text;
            mensaje[2] = textBox3.Text;
            Visitor visitor = new Visitor(ipBroker, puertoBroker, ipRegistry, puertoRegistry, llamador, mensaje);
            label4.Text = visitor.StartRConexion();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
