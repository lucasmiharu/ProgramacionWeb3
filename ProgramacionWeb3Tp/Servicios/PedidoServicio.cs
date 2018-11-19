using ProgramacionWeb3Tp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Data.Entity;

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
                             select u).FirstOrDefault();
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

        public void SetGustos(Pedido pedido, String[] Gustos)
        {
                                   
            List<GustoEmpanada> gustos = new List<GustoEmpanada>();
            GustoEmpanada gusto = new GustoEmpanada();
            
            for (int i = 0; i < Gustos.Count(); i++)
            {
                gusto = GetGustoEmpanada(int.Parse(Gustos[i]));
                gustos.Add(gusto);
            }
            pedido.GustoEmpanada = gustos;
            
            var entry = ctx.Entry(pedido);
            entry.State = EntityState.Modified;

            ctx.SaveChanges();               
        }

        public void EliminarGustosporPedido(Pedido pedido)
        {
            //GustoEmpanada Gusto = new GustoEmpanada();

           foreach (GustoEmpanada gusto in pedido.GustoEmpanada)            
            {                
             ctx.GustoEmpanada.Remove(gusto);
             ctx.SaveChanges();                
            }
        }

        public void EliminarInvitacionesporPedido(Pedido pedido)
        {
            foreach (InvitacionPedido invitacion in pedido.InvitacionPedido)
            {
                ctx.InvitacionPedido.Remove(invitacion);
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

        public List<Pedido> ObtenerInvitacionesPorUsuario(int usuarioId)
        {
            var pedidos = (from I in ctx.InvitacionPedido
                           join P in ctx.Pedido on I.IdPedido equals P.IdPedido
                           where I.IdUsuario == usuarioId
                           orderby I.IdPedido ascending
                           select P)
                            .ToList();
            return pedidos;
        }


        public List<GustoEmpanada> GetGustoEmpanadas()
        {
            var gustos = (from l in ctx.GustoEmpanada
                          orderby l.Nombre ascending
                           select l)
                            .ToList();

            return gustos;
        }

        public GustoEmpanada GetGustoEmpanada(int id)
        {
            var gusto = (from l in ctx.GustoEmpanada
                          where l.IdGustoEmpanada==id                          
                          select l)
                            .FirstOrDefault();

            return gusto;
        }

        public Pedido EditarPedidoExistente(Pedido pedido)
        {
            Pedido pedidoActualizado = ObtenerPedidoPorId(pedido.IdPedido);

            pedidoActualizado.NombreNegocio = pedido.NombreNegocio;
            pedidoActualizado.Descripcion = pedido.Descripcion;
            pedidoActualizado.PrecioDocena = pedido.PrecioDocena;
            pedidoActualizado.PrecioUnidad = pedido.PrecioUnidad;          
            pedidoActualizado.FechaModificacion = DateTime.Now;

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
     
    }
}
