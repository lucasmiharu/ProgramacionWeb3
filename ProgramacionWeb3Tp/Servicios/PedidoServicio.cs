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

        public Pedido ObtenerPedidoPorId(int? id)
        {
            Pedido pedido = (from u in ctx.Pedido
                             where u.IdPedido == id
                             select u)
                               .FirstOrDefault();
            return pedido;
        }

        public Pedido CrearPedido(Pedido dataPedido, Usuario usuarioActual)
        {

            Pedido nuevoPedido = new Pedido
            {
                IdPedido = dataPedido.IdPedido,
                NombreNegocio = dataPedido.NombreNegocio,
                Descripcion = dataPedido.Descripcion,
                PrecioUnidad = dataPedido.PrecioUnidad,
                PrecioDocena = dataPedido.PrecioDocena,
                FechaCreacion = DateTime.Now
            };

            usuarioActual.Pedido.Add(nuevoPedido);
            ctx.Pedido.Add(nuevoPedido);
            ctx.SaveChanges();

            return nuevoPedido;
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
