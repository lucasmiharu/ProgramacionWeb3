using ProgramacionWeb3Tp.Models;
using ProgramacionWeb3Tp.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProgramacionWeb3Tp.Controllers
{
    public class PedidosController : Controller
    {
        private readonly PedidoServicio _pedidoServicio = new PedidoServicio();
        private readonly UsuarioServicio _usuarioServicio = new UsuarioServicio();

        [HttpGet]
        public ActionResult iniciar()
        {
            
            Pedido pedido = new Pedido();
            List<Usuario> invitados = _usuarioServicio.GetAll();
            ViewBag.invitados = invitados;
                     
            return View(pedido);
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]  //Para prevenir ataques CSRF
        public ActionResult RegistrarPedido(Pedido pedido)
        {
            Pedido nuevo;


            if (ModelState.IsValid &&  Request["SelecInvitados"]!=null)
            {
                                
               nuevo= _pedidoServicio.CrearPedido(pedido, Request["SelecInvitados"].Split(','));
                _pedidoServicio.SetInvitados(nuevo, Request["SelecInvitados"].Split(','));

                ViewBag.mensaje = "Se genero el pedido nro: " + nuevo.IdPedido;
                return View("iniciado");

            }
            else
            {
                List<Usuario> invitados = _usuarioServicio.GetAll();
                ViewBag.invitados = invitados;
                return View("iniciar",pedido);
              

            }
        }

    }
}