using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Confluent.Kafka;

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
                visitor.SolicitudAccesoKafka();
                label3.Text = "Enviada solicitud a Engine...";
                if (visitor.RecibirAforoKafka())
                {
                    visitor.SolicitarMapaKafka();
                    String mapa = visitor.RecibirMapaKafka();
                    Thread.Sleep(1 * 1000);
                    InteriorParque ip = new InteriorParque(ipBroker, puertoBroker, ipRegistry, puertoRegistry, mapa);
                    ip.Show();
                } else
                {
                    Thread.Sleep(2 * 1000);
                    label3.Text = "Parque lleno.";
                }
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string url = "http://" + ipRegistry + ":44332/api/PerfilPersonal";
            var perfilPersonal = new PerfilPersonalDto();
            perfilPersonal.Usuario = label1.Text;
            perfilPersonal.Password = label2.Text;
            string resultado = Send<PerfilPersonalDto>(url, perfilPersonal, "GET");
            label3.Text = resultado;
        }

        public string Send<T>(string url, T objectRequest, string method)
        {
            string result = "";

            try
            {

                JavaScriptSerializer js = new JavaScriptSerializer();

                //serializamos el objeto
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(objectRequest);

                //peticion
                WebRequest request = WebRequest.Create(url);
                //headers
                request.Method = method;
                request.PreAuthenticate = true;
                request.ContentType = "application/json;charset=utf-8'";
                request.Timeout = 10000; //esto es opcional

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        result = stream.ToString();
                        Console.WriteLine(result);
                    }
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

            }
            catch (Exception e)
            {
                result = e.Message;

            }

            return result;
        }

        private void IniciarSesion_Load(object sender, EventArgs e)
        {

        }
    }
}
