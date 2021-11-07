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
    public partial class InteriorParque : Form
    {
        String ipBroker;
        String puertoBroker;
        String ipRegistry;
        String puertoRegistry;
        String mapa;
        public InteriorParque(string ipB, string puertoB, string ipR, string puertoR,String m)
        {
            ipBroker = ipB;
            puertoBroker = puertoB;
            ipRegistry = ipR;
            puertoRegistry = puertoR;
            mapa = m;
            InitializeComponent();
        }

        private void InteriorParque_Load(object sender, EventArgs e)
        {
            label1.Text = mapa;
            
        }

        private void InteriorParque_FormClosing(object sender, FormClosingEventArgs e)
        {
            String llamador = String.Empty;
            String[] mensaje = new String[1];
            Visitor visitor = new Visitor(ipBroker, puertoBroker, ipRegistry, puertoRegistry, llamador, mensaje);
            visitor.SalirParqueKafka();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Visitor visitor = new Visitor(ipBroker, puertoBroker, ipRegistry, puertoRegistry, String.Empty, new String[1]);
            /*Thread th1 = new Thread(visitor.SolicitarMapaKafka);
            th1.Start();*/
            visitor.SolicitarMapaKafka();
            //label1.Text = visitor.RecibirMapaKafka();
            Thread.Sleep(1 * 1000);
        }
    }
}
