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
    public partial class InteriorParque : Form
    {
        String ipBroker;
        String puertoBroker;
        String ipRegistry;
        String puertoRegistry;
        public InteriorParque(string ipB, string puertoB, string ipR, string puertoR)
        {
            ipBroker = ipB;
            puertoBroker = puertoB;
            ipRegistry = ipR;
            puertoRegistry = puertoR;
            InitializeComponent();
        }

        private void InteriorParque_Load(object sender, EventArgs e)
        {

        }

        private void InteriorParque_FormClosing(object sender, FormClosingEventArgs e)
        {
            String llamador = String.Empty;
            String[] mensaje = new String[1];
            Visitor visitor = new Visitor(ipBroker, puertoBroker, ipRegistry, puertoRegistry, llamador, mensaje);
            visitor.SalirParqueKafka();
        }
    }
}
