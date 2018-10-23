using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pedido_Empanadas.Models
{
    public class Pedido
    {

        public int IdPedido { get; set; }

        public int IdUsuarioResponsable { get; set; }

        public string NombreNegocio { get; set; }

        public string Descripcion { get; set; }

        public int IdEstadoPedido { get; set; }

        public int PrecioUnidad { get; set; }

        public int PrecioDocena { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }
    }
}