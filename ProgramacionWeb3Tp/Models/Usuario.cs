using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pedido_Empanadas.Models
{
    public class Usuario
    {

        public  int IdUsuario { get; set; }

        public string Email { get; set; }

        public char Password { get; set; }
    }
}