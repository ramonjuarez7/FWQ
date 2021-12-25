using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_Registry.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilPersonalController : ControllerBase
    {
        static String[] usuarios = new String[1000];
        static String userpath = Path.GetFullPath("..\\usuarios.txt");

        public static (int, String) BuscarUsuario(String alias)
        {
            String password = String.Empty;
            int lineToEdit = -1;
            StreamReader sr = System.IO.File.OpenText(userpath);
            String[] spliter;
            String line;
            bool existe = false;
            for (int i = 0; (line = sr.ReadLine()) != null && !existe; i++)
            {
                spliter = line.Split(';');
                if (spliter[0].Equals(alias))
                {
                    existe = true;
                    lineToEdit = i;
                    password = spliter[2];
                }
            }
            sr.Close();
            return (lineToEdit, password);
        }

        public void ObtenerUsuarios()
        {
            StreamReader sr = System.IO.File.OpenText(userpath);
            String line;
            for (int i = 0; i < usuarios.Length; i++)
            {
                usuarios[i] = "";
            }

            for (int i = 0; (line = sr.ReadLine()) != null; i++)
            {
                usuarios[i] = line;
            }
            sr.Close();

        }

        static void lineRemover(int lte)
        {
            string[] arrLine = System.IO.File.ReadAllLines(userpath);
            for(int i = lte; i < arrLine.Length - 1; i++)
            {
                arrLine[i] = arrLine[i + 1];
            }
            //arrLine[lte] = newText;
            System.IO.File.WriteAllLines(userpath, arrLine);
        }
        static void lineChanger(string newText, int lte)
        {
            string[] arrLine = System.IO.File.ReadAllLines(userpath);
            arrLine[lte] = newText;
            System.IO.File.WriteAllLines(userpath, arrLine);
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            ObtenerUsuarios();
            if(id < usuarios.Length) {
                if (!usuarios[id].Equals("")) {
                    String[] spliter = usuarios[id].Split(';');
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Usuario: " + spliter[0] + "\nNombre: " + spliter[1] + "\n");
                    return sb.ToString();
                } 
            }
            return "Usuario Inexistente.";
        }

        [HttpGet]
        public string Get()
        {
            ObtenerUsuarios();
            StringBuilder sb = new StringBuilder();
            sb.Append("Información de todos los usuarios registrados: \n");
            for (int i = 0; i < usuarios.Length && !usuarios[i].Equals(""); i++)
            {
                String[] spliter = usuarios[i].Split(';');
                sb.Append("Usuario: " + spliter[0] + "\n");
            }     
            return sb.ToString();
        }

        [HttpPost]
        public string Post(/*string nombre, string apellido, string apellido*/ PerfilPersonalDto perfilPersonal)
        {
            int linea;
            String passwd, mensaje;
            (linea,passwd) = BuscarUsuario(perfilPersonal.Usuario);
            if(linea == -1)
            {
                StreamWriter sw = System.IO.File.AppendText(userpath);
                sw.WriteLine(perfilPersonal.Usuario + ";" + perfilPersonal.Nombre + ";" + perfilPersonal.Password + ";");
                mensaje = "Usuario " + perfilPersonal.Usuario + " creado con éxito. ¡Bienvenido a FWQ, " +
                perfilPersonal.Nombre + "!";
                sw.Close();
            } else
            {
                mensaje = "¡Ya existe un usuario con ese alias!";
            }
           
            return mensaje;
        }

        [HttpDelete]
        public string Delete(PerfilPersonalDto perfilPersonal)
        {
            int linea;
            String passwd, mensaje;
            (linea, passwd) = BuscarUsuario(perfilPersonal.Usuario);
            if (passwd.Equals(perfilPersonal.Password))
            {
                lineRemover(linea);
                mensaje = "Usuario " + perfilPersonal.Usuario + " eliminado con éxito.";
            }
            else
            {
                mensaje = "La contraseña es incorrecta, ¡no se puede eliminar el usuario!";
            }
            return mensaje;
        }

        [HttpPut]
        public string Put(PerfilPersonalDto perfilPersonal)
        {
            int linea;
            String passwd, mensaje;
            (linea, passwd) = BuscarUsuario(perfilPersonal.Usuario);
            if (passwd.Equals(perfilPersonal.Password) && linea != -1)
            {
                lineChanger(perfilPersonal.NuevoUsuario + ";" + perfilPersonal.NuevoNombre + ";" +
                    perfilPersonal.NuevoPassword + ";",linea);
                mensaje = "Usuario " + perfilPersonal.Usuario + " modificado con éxito.";
            }
            else
            {
                mensaje = "La contraseña o el usuario son incorrectos, ¡no se puede modificar el usuario!";
            }
            return mensaje;
        }

        public class PerfilPersonalDto
        {
            public string Usuario { get; set; }
            public string Nombre { get; set; }
            public string Password { get; set; }
            public string NuevoUsuario { get; set; }
            public string NuevoNombre { get; set; }
            public string NuevoPassword { get; set; }

        }
    }
}
