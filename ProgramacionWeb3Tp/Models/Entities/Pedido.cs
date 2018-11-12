using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ProgramacionWeb3Tp.Models.Metadata;

namespace ProgramacionWeb3Tp.Models
{
    [MetadataType(typeof(PedidoMetadata))]
    public partial class Pedido
    {
        public Pedido(Pedido pedido)
        {
            this.Descripcion = pedido.Descripcion;         
            this.FechaCreacion = DateTime.Now;
            //this.FechaModificacion -- no se carga por q es un pedido nuevo
            //   this.GustoEmpanada  no se carga por q es un pedido nuevo
            this.IdEstadoPedido = 1; //estado Abierto                                    
            this.IdUsuarioResponsable = ClsSesion.GetUsuarioLogueado().IdUsuario; //el usuario logueado creo el pedido            
            this.NombreNegocio = pedido.NombreNegocio;
            this.PrecioDocena = pedido.PrecioDocena;
            this.PrecioUnidad = pedido.PrecioUnidad;           

        }
    }
}

