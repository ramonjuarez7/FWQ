using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Front3
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void botonusuarios_Click(object sender, EventArgs e)
        {
            string url = "http://localhost:44332/api/PerfilPersonal";

            string resultado = Send(url, "GET");
            Mapa.Text = resultado;
        }

        protected void botonmapa_Click(object sender, EventArgs e)
        {
            string url = "http://localhost:44332/api/Mapa";

            string resultado = Send(url, "GET");
            Mapa.Text = resultado;
            
        }

        /*
        public string Send<T>(string url, T objectRequest, string method = "POST")
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
                /*request.PreAuthenticate = true;
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
                /*
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
        }*/

        public string Send(string url, string method = "GET")
        {
            string result = "";

            try
            {

                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //peticion
                WebRequest request = WebRequest.Create(url);
                //headers
                request.Method = method;
                /*request.PreAuthenticate = true;
                request.ContentType = "application/json;charset=utf-8'";
                request.Timeout = 10000; //esto es opcional*/

                /*using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        result = stream.ToString();
                        Console.WriteLine(result);
                    }
                }*/
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