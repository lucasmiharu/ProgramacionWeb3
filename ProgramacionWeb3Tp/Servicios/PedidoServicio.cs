using ProgramacionWeb3Tp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using ProgramacionWeb3Tp.Helper;

namespace ProgramacionWeb3Tp.Servicios
{
    public class PedidoServicio
    {
        private static Pedidos ctx = new Pedidos();
        private readonly UsuarioServicio _usuarioServicio = new UsuarioServicio();
     
        public Pedido ObtenerPedidoPorId(int id)
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

        internal int ObtenerInvitacionesConfirmadas(int idUsuario)
        {
            int Confirmadas = (from l in ctx.InvitacionPedido
                               where l.IdPedido == 2
                               select l).Count();

            
            return Confirmadas;
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
                Invitacion.Token = new Guid(new Md5Hash().GetMD5((idUsr.ToString() + pedido.FechaCreacion)));
                Invitacion.Completado = false;


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

        internal List<Usuario> ObtenerUsuariosInvitadosDePedido(Pedido pedido)
        {
            var query =
                (from ip in ctx.InvitacionPedido
                 join p in ctx.Pedido on ip.IdPedido equals p.IdPedido
                 join u in ctx.Usuario on ip.IdUsuario equals u.IdUsuario
                 where ip.IdPedido == pedido.IdPedido
                 select
                    u).Distinct().ToList();
            return query;
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
                
                List<int> gustoEmpanadas = new List<int>();
                List<int> invitacionPedido = new List<int>();
                List<int> invitacionPedidoGustoEmpanadaUsuarioIds = new List<int>();

                //Recorremos la lista de los id
                foreach (var gusto in pedido.GustoEmpanada)
                {
                    gustoEmpanadas.Add(gusto.IdGustoEmpanada);
                }

                foreach (var invitacion in pedido.InvitacionPedido)
                {
                    invitacionPedido.Add(invitacion.IdInvitacionPedido);
                }
                foreach (var invitacionPedidoGustoEmpanadaUsuario in pedido.InvitacionPedidoGustoEmpanadaUsuario)
                {
                    invitacionPedidoGustoEmpanadaUsuarioIds.Add(invitacionPedidoGustoEmpanadaUsuario.IdInvitacionPedidoGustoEmpanadaUsuario);
                }

                //Eliminamos los id recorridos 
                foreach (var idGustos in gustoEmpanadas)
                {
                    var gustoEliminar = ctx.GustoEmpanada.FirstOrDefault(g => g.IdGustoEmpanada == idGustos);
                    pedido.GustoEmpanada.Remove(gustoEliminar);
                }

                foreach (var idInvitacion in invitacionPedido)
                {
                    var invitacionEliminar = ctx.InvitacionPedido.FirstOrDefault(i => i.IdInvitacionPedido == idInvitacion);
                    ctx.InvitacionPedido.Remove(invitacionEliminar);
                }

                foreach (var idInvitacionPedidoGustoEmpanadaUsuario in invitacionPedidoGustoEmpanadaUsuarioIds)
                {
                    var invitacionPedidoGustoEmpanadaUsuarioEliminar = ctx.InvitacionPedidoGustoEmpanadaUsuario.FirstOrDefault(i => i.IdInvitacionPedidoGustoEmpanadaUsuario == idInvitacionPedidoGustoEmpanadaUsuario);
                    ctx.InvitacionPedidoGustoEmpanadaUsuario.Remove(invitacionPedidoGustoEmpanadaUsuarioEliminar);
                }
                //salvamos
                ctx.Pedido.Remove(pedido);
                ctx.SaveChanges();

            }
        }

        public Pedido FinalizarPedido(int pedidoId)
        {
            Pedido pedidoActualizado = ObtenerPedidoPorId(pedidoId);


            pedidoActualizado.IdEstadoPedido = 2;
            pedidoActualizado.FechaModificacion = DateTime.Now;
            ctx.SaveChanges();

             return pedidoActualizado;
           
        }
       
        public Pedido DetallePedido(int idPedido)
        {
            Pedido detallePedido = ObtenerPedidoPorId(idPedido);

            return detallePedido;

        }
        
        public void EnviarMail(List<Usuario> usuariosAEnviarInvitacion, Pedido pedido)
        {

            Pedido pedidoEncontrado = ctx.Pedido.SingleOrDefault(p => p.IdPedido == pedido.IdPedido);

            foreach (InvitacionPedido invitado in pedidoEncontrado.InvitacionPedido)
            {
                EmailService.EnviarEmailInvitados(invitado);

            }


            //    foreach (Usuario invitado in usuariosAEnviarInvitacion)
            //{
            //    Usuario usuarioEncontrado = ctx.Usuario.SingleOrDefault(x => x.IdUsuario == invitado.IdUsuario);
               


            //    pedidoEncontrado.InvitacionPedido

            //    InvitacionPedido invitacionPedido = new InvitacionPedido
            //    {
            //        Pedido = pedidoEncontrado,
            //        Usuario = usuarioEncontrado,
            //        Token = new Guid(new Md5Hash().GetMD5((usuarioEncontrado.Email + pedidoEncontrado.FechaCreacion))),
            //        Completado = false,
            //    };
                //ctx.InvitacionPedido.Add(invitacionPedido);
                //ctx.SaveChanges();
               
           // }
        }

                public void EnviarInvitacionesDesdeUnaListaDeUsuarios(List<Usuario> usuarios, Pedido pedido)
        {
            foreach (Usuario invitado in usuarios)
            {
                Usuario usuarioEncontrado = ctx.Usuario.SingleOrDefault(x => x.IdUsuario == invitado.IdUsuario);
                Pedido pedidoEncontrado = ctx.Pedido.SingleOrDefault(p => p.IdPedido == pedido.IdPedido);
                InvitacionPedido invitacionPedido = new InvitacionPedido
                {
                    Pedido = pedidoEncontrado,
                    Usuario = usuarioEncontrado,
                    Token = new Guid(new Md5Hash().GetMD5((usuarioEncontrado.Email + pedidoEncontrado.FechaCreacion))),
                    Completado = false,
                };
                ctx.InvitacionPedido.Add(invitacionPedido);
                ctx.SaveChanges();
                EmailService.EnviarEmailInvitados(invitacionPedido);
            }
        }        

        public Usuario BuscarUsuarioById(int idUsuario)
        {
            Usuario usuarioEncontrado = ctx.Usuario.SingleOrDefault(x => x.IdUsuario == idUsuario);
            return usuarioEncontrado;
        }

        public Pedido ObtenerPedidoByToken(Guid token)
        {
            Pedido pedido = ctx.InvitacionPedido.Where(i => i.Token == token).Select(p => p.Pedido).FirstOrDefault();
            return pedido;
        }

        public Boolean InvitacionPedidoUsuarioIsTrue(int idPedido, Usuario usuario)
        {
            var query = (from ip in ctx.InvitacionPedido
                         where ip.IdUsuario == usuario.IdUsuario && ip.IdPedido == idPedido
                         select ip).ToList();

            if (query.Count > 0)
            {
                return true;
            }
            return false;
        }

        public int ObtenerEnviarInvitacion(Pedido pedido)
        {
            int envioInvitacion = pedido.IdPedido;

            return envioInvitacion;
        }

        public List<Usuario> DeterminarEnviosDeInvitacionSegunEstado(Pedido pedido)
        {
            int IdEnviarInvitacion = ObtenerEnviarInvitacion(pedido);

            List<Usuario> usuariosAInvitar = new List<Usuario>();

            switch (IdEnviarInvitacion)
            {
                case 1:
                    //A nadie
                    break;
                case 2:
                    //Re enviar a todos
                    usuariosAInvitar = ObtenerTodosLosUsuariosInvitados(pedido);
                    break;
                case 3:
                    //Enviar solo a los nuevos
                    usuariosAInvitar = ObtenerLosUsuariosQueAntesNoEstabanInvitados(pedido);
                    break;
                case 4:
                    //Re enviar a los que no eligieron gustos
                    usuariosAInvitar = ObtenerLosUsuariosInvitadosQueNoEligieronGustos(pedido);
                    break;
            }

            return usuariosAInvitar;
        }

        public List<Usuario> ObtenerTodosLosUsuariosInvitados(Pedido pedido)
        {
            var query =
                (from ip in ctx.InvitacionPedido
                 join p in ctx.Pedido on ip.IdPedido equals p.IdPedido
                 join u in ctx.Usuario on ip.IdUsuario equals u.IdUsuario
                 where ip.IdPedido == pedido.IdPedido
                 select
                    u).Distinct().ToList();
            return query;
        }

        public List<Usuario> ObtenerLosUsuariosQueAntesNoEstabanInvitados(Pedido pedido)
        {
            List<Usuario> usuariosAInvitar = new List<Usuario>();

            List<Usuario> usuariosListados = new List<Usuario>();

            Pedido pedidoAEditar = ObtenerPedidoPorId(pedido.IdPedido);

            //int[] invitados = Array.ConvertAll(pedido.GetValues("invitados"), int.Parse);

            //foreach (var usuario in invitados)
            //{
            //    Usuario usuarioBuscado = BuscarUsuarioById(usuario);
            //    usuariosListados.Add(usuarioBuscado);
            //}

            foreach (Usuario usuario in usuariosListados)
            {
                foreach (InvitacionPedido invitacionPedido in pedidoAEditar.InvitacionPedido)
                {
                    if (usuario.InvitacionPedido.Contains(invitacionPedido))
                    {
                        break;
                    }
                    else
                    {
                        usuariosAInvitar.Add(usuario);
                        break;
                    }
                }
            }

            return usuariosAInvitar;
        }

        private List<Usuario> ObtenerLosUsuariosInvitadosQueNoEligieronGustos(Pedido pedido)
        {
            List<Usuario> usuariosAInvitar = new List<Usuario>();

            List<Usuario> usuariosListados = new List<Usuario>();

            Pedido pedidoAEditar = ObtenerPedidoPorId(pedido.IdPedido);

            //int[] invitados = Array.ConvertAll(pedido.InvitacionPedido.GetValues("invitados"), int.Parse);

            //foreach (var usuario in invitados)
            //{
            //    Usuario usuarioBuscado = BuscarUsuarioById(usuario);
            //    usuariosListados.Add(usuarioBuscado);
            //}

            foreach (Usuario usuario in usuariosListados)
            {
                if (usuario.InvitacionPedido.Where(u => u.IdPedido == pedidoAEditar.IdPedido).Select(i => i.Completado).FirstOrDefault() == true)
                {

                }
                else
                {
                    usuariosAInvitar.Add(usuario);
                }
            }

            return usuariosAInvitar;
        }
    }
}

