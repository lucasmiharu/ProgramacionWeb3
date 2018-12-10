using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace appiPedidos.Models
{
    public class UsuarioGustos
    {       
        public UsuarioGustos()
        {
            this.GustosEmpanadasCantidad = new HashSet<GustoCantidad>();          
        }

        public int IdUsuario { get; set; }
        public Guid Token { get; set; }
        public virtual ICollection<GustoCantidad> GustosEmpanadasCantidad { get; set; }     
    }

    public class GustoCantidad
    {
        public int IdGustoEmpanada { get; set; }
        public int Cantidad { get; set; }
    }


}