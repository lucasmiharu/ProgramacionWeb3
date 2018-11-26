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
        public ActionResult Pedidos(String Copia)
        {

            List<Pedido> pedidos = new List<Pedido>();

            pedidos = _pedidoServicio.ObtenerPedidosPorUsuario(ClsSesion.GetUsuarioLogueado().IdUsuario);

            if (Copia == "S")
                ViewBag.ModoCopia = "S";
            else
                ViewBag.ModoCopia = "N";

            return View("Pedidos", pedidos);
        }

        [HttpGet]
        public ActionResult Invitaciones()
        {

            List<Pedido> pedidos = new List<Pedido>();

            pedidos = _pedidoServicio.ObtenerInvitacionesPorUsuario(ClsSesion.GetUsuarioLogueado().IdUsuario);

            return View("invitaciones", pedidos);
        }


        [HttpGet]
        public ActionResult Copiar(int id)
        {
            Pedido pedido = _pedidoServicio.ObtenerPedidoPorId(id);

            List<Usuario> invitados = _usuarioServicio.GetAll();
            List<GustoEmpanada> gustos = _pedidoServicio.GetGustoEmpanadas();
            ViewBag.invitados = invitados;
            ViewBag.gustos = gustos;

            return View("copiar", pedido);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]  //Para prevenir ataques CSRF
        public ActionResult RegistrarPedido(Pedido pedido)
        {
            Pedido nuevo;


            if (ModelState.IsValid && Request["SelecInvitados"] != null)
            {

                nuevo = _pedidoServicio.CrearPedido(pedido, Request["SelecInvitados"].Split(','));
                _pedidoServicio.SetInvitados(nuevo, Request["SelecInvitados"].Split(','));
                _pedidoServicio.SetGustos(nuevo, Request["SelecGustos"].Split(','));

                ViewBag.mensaje = "Se genero el pedido nro: " + nuevo.IdPedido;
                return RedirectToAction("Pedidos");

            }
            else
            {
                List<Usuario> invitados = _usuarioServicio.GetAll();
                List<GustoEmpanada> gustos = _pedidoServicio.GetGustoEmpanadas();
                ViewBag.invitados = invitados;
                ViewBag.gustos = gustos;
                return View("iniciar", pedido);
            }
        }

        [HttpPost]
        public ActionResult EliminarPedido(int pedidoId)
        {
            PedidoServicio PS = new PedidoServicio();
            Pedido pedido = new Pedido();

            PS.EliminarPedido(pedidoId);
            return View("Pedidos", pedido);
        }


        [HttpGet]
        public ActionResult EditarPedido(int id)
        {
            Pedido pedido = _pedidoServicio.ObtenerPedidoPorId(id);

            List<Usuario> invitados = _usuarioServicio.GetAll();
            List<GustoEmpanada> gustos = _pedidoServicio.GetGustoEmpanadas();
            ViewBag.invitados = invitados;
            ViewBag.gustos = gustos;

            return View("editar", pedido);
        }

    
        public ActionResult ElegirGustos(int id)

        {
            Pedido pedido = _pedidoServicio.ObtenerPedidoPorId(id);         

            return View("elegir", pedido);

        }    

        [HttpPost]
        [ValidateAntiForgeryToken]  //Para prevenir ataques CSRF
        public ActionResult EditarPedido(Pedido pedido)
        {         

            //si eligio invitados y gustos sigo
            if (ModelState.IsValid && Request["SelecInvitados"] != null && Request["SelecGustos"] != null)
            {
                //solo editar el pedido
               Pedido nuevo = _pedidoServicio.EditarPedidoExistente(pedido);

                _pedidoServicio.EliminarInvitacionesporPedido(pedido);
                _pedidoServicio.EliminarGustosporPedido(pedido);                
              
                _pedidoServicio.SetInvitados(nuevo, Request["SelecInvitados"].Split(','));
                                
                _pedidoServicio.SetGustos(nuevo, Request["SelecGustos"].Split(','));

                ViewBag.mensaje = "Se edito el pedido nro: " + nuevo.IdPedido;
                return RedirectToAction("Pedidos");
            }
            else
            {
                List<Usuario> invitados = _usuarioServicio.GetAll();
                List<GustoEmpanada> gustos = _pedidoServicio.GetGustoEmpanadas();
                ViewBag.invitados = invitados;
                ViewBag.gustos = gustos;
                return View("iniciar", pedido);
            }
        }



    }
}
