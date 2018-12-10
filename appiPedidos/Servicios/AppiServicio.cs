using appiPedidos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace appiPedidos.Servicios
{

    public class AppiServicio
    {

        private static Entities condb = new Entities();

        private static Entities ctx = new Entities();


        //TODO: Elegir service por json
        public MensajeAppi ElegirGustos(UsuarioGustos GustosUsuario)
        {
            MensajeAppi msj = new MensajeAppi();
            try
            {
                Pedido pedido = ObtenerPedidoPorId(ctx.InvitacionPedido.Where(t => t.Token == GustosUsuario.Token && t.IdUsuario == GustosUsuario.IdUsuario).FirstOrDefault().IdPedido);

                foreach (var gustoSolicitados in GustosUsuario.GustosEmpanadasCantidad)
                {
                    if (pedido.GustoEmpanada.Where(g => g.IdGustoEmpanada == gustoSolicitados.IdGustoEmpanada).Count() == 0)
                    {
                        msj.Resultado = "ERROR";
                        msj.Mensaje = "No se pudo efectuar la operacion porque uno o mas pedidos no estan disponibles";
                        return msj;
                    }

                }

                if (pedido.EstadoPedido.Nombre == "Cerrado")
                {
                    msj.Resultado = "ERROR";
                    msj.Mensaje = "No se pudo efectuar la operacion porque el pedido se encuentra cerrado";
                    return msj;
                }

                foreach (var gusto in pedido.GustoEmpanada)
                {
                    try
                    {

                        if (pedido.InvitacionPedidoGustoEmpanadaUsuario.Where(p => p.IdUsuario == GustosUsuario.IdUsuario && p.IdGustoEmpanada == gusto.IdGustoEmpanada).Count() > 0)
                        {
                            ctx.InvitacionPedidoGustoEmpanadaUsuario.Remove(ctx.InvitacionPedidoGustoEmpanadaUsuario.Where(i => i.GustoEmpanada.IdGustoEmpanada == gusto.IdGustoEmpanada && i.IdUsuario == GustosUsuario.IdUsuario && i.IdPedido == pedido.IdPedido).FirstOrDefault());

                        }

                        var cantidadEmpanada = GustosUsuario.GustosEmpanadasCantidad.Where(c => c.IdGustoEmpanada == gusto.IdGustoEmpanada).FirstOrDefault().Cantidad;
                        if (cantidadEmpanada != 0)
                        {

                            InvitacionPedidoGustoEmpanadaUsuario ipgeu = new InvitacionPedidoGustoEmpanadaUsuario
                            {
                                Cantidad = cantidadEmpanada,
                                GustoEmpanada = gusto,
                                IdUsuario = GustosUsuario.IdUsuario,
                            };


                            pedido.InvitacionPedidoGustoEmpanadaUsuario.Add(ipgeu);
                        }


                    }
                    catch (Exception e)
                    {
                        if (pedido.InvitacionPedidoGustoEmpanadaUsuario.Where(p => p.IdUsuario == GustosUsuario.IdUsuario && p.IdGustoEmpanada == gusto.IdGustoEmpanada).Count() > 0)
                        {
                            ctx.InvitacionPedidoGustoEmpanadaUsuario.Remove(ctx.InvitacionPedidoGustoEmpanadaUsuario.Where(i => i.GustoEmpanada.IdGustoEmpanada == gusto.IdGustoEmpanada && i.IdUsuario == GustosUsuario.IdUsuario && i.IdPedido == pedido.IdPedido).FirstOrDefault());
                        }
                        msj.Resultado = "ERROR";
                        msj.Mensaje = "No se pudo efectuar la operacion porque " + e.ToString();
                        return msj;

                    }

                }


                var invitacion = ctx.InvitacionPedido.Where(i => i.IdPedido == pedido.IdPedido && i.IdUsuario == GustosUsuario.IdUsuario).FirstOrDefault();
                invitacion.Completado = true;
                ctx.SaveChanges();
                msj.Resultado = "OK";
                msj.Mensaje = "Gustos elegidos satisfactoriamente";
                return msj;
            }
            catch (NullReferenceException)
            {
                msj.Resultado = "ERROR";
                msj.Mensaje = "No se pudo efectuar la operacion porque el usuario es invalido";
                return msj;

            }

        }



        public InvitacionPedido GetInvitacionPedidoPorToken(string token, int idusuario)
        {
            var invitacion = (from u in condb.InvitacionPedido
                              where u.Token == Guid.Parse(token) && u.IdUsuario == idusuario
                              select u).FirstOrDefault();

            return invitacion;

        }


        public void SetGustosEmpanadasCantidad(InvitacionPedidoGustoEmpanadaUsuario datos)
        {

            condb.InvitacionPedidoGustoEmpanadaUsuario.Add(datos);
            condb.SaveChanges();

        }


        public void DeleteGustosEmpanadasCantidad(InvitacionPedidoGustoEmpanadaUsuario datos)
        {

            condb.InvitacionPedidoGustoEmpanadaUsuario.Remove(datos);
            condb.SaveChanges();

        }

        public Pedido ObtenerPedidoPorId(int id)
        {
            Pedido pedido = (from u in ctx.Pedido
                             where u.IdPedido == id
                             select u).FirstOrDefault();
            return pedido;
        }



    }
}
