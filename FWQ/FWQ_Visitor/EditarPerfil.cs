using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
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

        private void button2_Click(object sender, EventArgs e)
        {
            string url = "http://" + ipRegistry + ":44332/api/PerfilPersonal";
            var perfilPersonal = new PerfilPersonalDto();
            perfilPersonal.Usuario = alias1.Text;
            perfilPersonal.Password = passwd1.Text;
            perfilPersonal.NuevoUsuario = alias2.Text;
            perfilPersonal.NuevoPassword = passwd2.Text;
            perfilPersonal.NuevoNombre = name2.Text;
            string resultado = Send<PerfilPersonalDto>(url, perfilPersonal, "PUT");
            label4.Text = resultado;
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
    }
}
