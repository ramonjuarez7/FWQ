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
    public partial class Inicio : Form
    {
        String ipBroker;
        String puertoBroker;
        String ipRegistry;
        String puertoRegistry;
        public Inicio(string ipB, string puertoB, string ipR, string puertoR)
        {
            ipBroker = ipB;
            puertoBroker = puertoB;
            ipRegistry = ipR;
            puertoRegistry = puertoR;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 CambioF = new Form1(ipBroker, puertoBroker, ipRegistry, puertoRegistry);
            CambioF.Show();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {

        }
    }
}
