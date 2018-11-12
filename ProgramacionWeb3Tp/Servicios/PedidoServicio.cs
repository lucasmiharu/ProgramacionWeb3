using ProgramacionWeb3Tp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;


namespace ProgramacionWeb3Tp.Servicios
{
    public class PedidoServicio
    {
        private static Pedidos ctx = new Pedidos();
        private readonly UsuarioServicio _usuarioServicio = new UsuarioServicio();

        public Pedido ObtenerPedidoPorId(int? id)
        {
            Pedido pedido = (from u in ctx.Pedido
                             where u.IdPedido == id
                             select u)
                               .FirstOrDefault();
            return pedido;
        }

        public int GetNuevoId()
        {
            var result = ctx.Pedido.Max(p => p.IdPedido);

            return result += 1;
        }

        public Pedido CrearPedido(Pedido dataPedido, String[] Invitados)
        {   
            Pedido nuevoPedido = new Pedido(dataPedido);

            nuevoPedido.IdPedido = GetNuevoId();
            
            ctx.Pedido.Add(nuevoPedido);
            ctx.SaveChanges();
           
            return nuevoPedido;
            
        }


        public void SetInvitados(Pedido pedido, String[] Invitados)
        {
            int idUsr;
                        
            InvitacionPedido Invitacion = new InvitacionPedido();
                                    
            for (int i = 0; i < Invitados.Count(); i++)
            {

                int.TryParse(Invitados[i], out idUsr);

                Invitacion.IdUsuario = idUsr;
                Invitacion.IdPedido = pedido.IdPedido;

                ctx.InvitacionPedido.Add(Invitacion);
                ctx.SaveChanges();

            }
        }
     

        public List<Pedido> ObtenerPedidosPorUsuario(int usuarioId)
        {
            var pedidos = (from l in ctx.Pedido
                            where l.IdUsuarioResponsable == usuarioId
                            orderby l.NombreNegocio ascending
                            select l)
                            .ToList();

            return pedidos;
        }

        public Pedido EditarPedidoExistente(Pedido pedido)
        {
            Pedido pedidoActualizado = ObtenerPedidoPorId(pedido.IdPedido);

            pedidoActualizado.NombreNegocio = pedido.NombreNegocio;
            pedidoActualizado.Descripcion = pedido.Descripcion;
            pedidoActualizado.PrecioDocena = pedido.PrecioDocena;
            pedidoActualizado.PrecioUnidad = pedido.PrecioUnidad;

            ctx.SaveChanges();

            return pedidoActualizado;
        }

        public void EliminarPedido(int pedidoId)
        {
            Pedido pedido = ObtenerPedidoPorId(pedidoId);

            if (pedido != null)
            {
                ctx.Pedido.Remove(pedido);

                ctx.SaveChanges();
            }
        }
        
       //private Pedidos condb = new Pedidos();

        //public void SavePedido(Pedido pedido)
        //{
        //    Pedido pedidoNuevo = null;
            
        //    pedidoNuevo = new Pedido();

        //    condb.Pedido.Add(pedido);
        //    condb.SaveChanges();

                   
        //}
    }
}
