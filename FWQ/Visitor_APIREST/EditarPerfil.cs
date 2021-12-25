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
            String llamador = "Editar perfil";           
            String[] mensaje = new string[5]; 
            mensaje[0] = alias1.Text;
            mensaje[1] = passwd1.Text;
            mensaje[2] = alias2.Text;
            mensaje[3] = name2.Text;
            mensaje[4] = passwd2.Text;
            Visitor visitor = new Visitor(ipBroker, puertoBroker, ipRegistry, puertoRegistry, llamador, mensaje);      
            label7.Text = visitor.StartRConexion();
        }
    }
}
