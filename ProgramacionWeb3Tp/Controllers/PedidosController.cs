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
            List<GustoEmpanada> gustos = _pedidoServicio.GetGustoEmpanadas();
            ViewBag.invitados = invitados;
            ViewBag.gustos = gustos;

            return View(pedido);
        }

        [HttpGet]        
        public ActionResult Pedidos()
        {

          List  <Pedido> pedidos = new List<Pedido>();

            pedidos = _pedidoServicio.ObtenerPedidosPorUsuario(ClsSesion.GetUsuarioLogueado().IdUsuario);
         
            return View("Pedidos", pedidos);
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
                _pedidoServicio.SetGustos(nuevo, Request["SelecGustos"].Split(','));

                ViewBag.mensaje = "Se genero el pedido nro: " + nuevo.IdPedido;
                return RedirectToAction("Pedidos");

            }
            else
            {
                List<Usuario> invitados = _usuarioServicio.GetAll();
                ViewBag.invitados = invitados;
                return View("iniciar",pedido);          

            }
        }
        [HttpPost]
        public ActionResult EliminarPedido(int id)
        {
            PedidoServicio PS = new PedidoServicio();
            Pedido pedido = new Pedido();

            PS.EliminarPedido(id);
            return View("Pedidos", pedido);
        }
    }
}