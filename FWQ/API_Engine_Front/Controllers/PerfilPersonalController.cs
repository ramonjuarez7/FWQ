using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Engine1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilPersonalController : ControllerBase
    {
        static String[] usuarios = new String[1000];
        static String mappath = Path.GetFullPath("..\\mapa.txt");
        static String userpath = Path.GetFullPath("..\\usuarios.txt");

        public void ObtenerUsuarios()
        {
            StreamReader sr = System.IO.File.OpenText(userpath);
            String line;
            for (int i = 0; (line = sr.ReadLine()) != null; i++)
            {
                usuarios[i] = line;
            }
            sr.Close();

        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            ObtenerUsuarios();
            String[] spliter = usuarios[id].Split(';');
            StringBuilder sb = new StringBuilder();
            sb.Append("Usuario: " + spliter[0] + "\nNombre: " + spliter[1] + "\n");
            return sb.ToString();
        }

        [HttpPost]
        public string Post(/*string nombre, string apellido, string apellido*/ PerfilPersonalDto perfilPersonal)
        {
            return perfilPersonal.Nombre;
        }

        public class PerfilPersonalDto
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
          

        }
    }
}
