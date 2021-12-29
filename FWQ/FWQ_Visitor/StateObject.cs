using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FWQ_Visitor
{
    class StateObject
    {
        // Size of receive buffer.  
        public const int BufferSize = 1024;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder sb = new StringBuilder();

        // Client socket.
        public Socket workSocket = null;
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
