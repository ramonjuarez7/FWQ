using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Engine1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilPersonalController : ControllerBase
    {
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return id switch
            {
                1 => "Ramom",
                2 => "Héctor",
                _ => throw new NotSupportedException("id no valido")
            };
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
