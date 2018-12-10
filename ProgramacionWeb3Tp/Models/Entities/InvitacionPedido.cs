using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgramacionWeb3Tp.Models.Entities
{
    public class InvitacionPedido
    {
       
            public int IdInvitacionPedido { get; set; }
            public int IdPedido { get; set; }
            public int IdUsuario { get; set; }
            public System.Guid Token { get; set; }
            public bool Completado { get; set; }

            public virtual Pedido Pedido { get; set; }
           // public virtual Usuario Usuario { get; set; }
    }
}
