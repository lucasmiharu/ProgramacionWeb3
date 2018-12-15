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
        static Usuario usuarioLogueado = new Usuario();
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
            try
            {
                pedidos = _pedidoServicio.ObtenerPedidosPorUsuario(ClsSesion.GetUsuarioLogueado().IdUsuario);
            }
            catch
            {
                return RedirectToAction("Index", "Home");

            }
            
            
            int confirmados = _pedidoServicio.ObtenerInvitacionesConfirmadas(ClsSesion.GetUsuarioLogueado().IdUsuario);
            ViewBag.InvitacionesConfirmadas = confirmados;

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


            if (ModelState.IsValid && Request["SelecInvitados"] != null && Request["SelecGustos"] != null )
            {

                nuevo = _pedidoServicio.CrearPedido(pedido, Request["SelecInvitados"].Split(','));
                _pedidoServicio.SetInvitados(nuevo, Request["SelecInvitados"].Split(','));
                _pedidoServicio.SetGustos(nuevo, Request["SelecGustos"].Split(','));

                nuevo = _pedidoServicio.ObtenerPedidoPorId(nuevo.IdPedido);

                _pedidoServicio.EnviarMail(_pedidoServicio.ObtenerTodosLosUsuariosInvitados(nuevo), nuevo);

                ViewBag.mensaje = "Se genero el pedido nro: " + nuevo.IdPedido;
                return RedirectToAction("Pedidos");

            }
            else
            {
                List<Usuario> invitados = _usuarioServicio.GetAll();
                List<GustoEmpanada> gustos = _pedidoServicio.GetGustoEmpanadas();
                ViewBag.invitados = invitados;
                ViewBag.gustos = gustos;
                ViewBag.mensaje = "Falta informacion para iniciar el pedido ";
                return View("iniciar", pedido);
            }
        }

        [HttpPost]
        public ActionResult EliminarPedido(int pedidoId)
        {
            PedidoServicio PS = new PedidoServicio();
            Pedido pedido = new Pedido();

            PS.EliminarPedido(pedidoId);
            //return View("Pedidos", pedido);
            return RedirectToAction("Pedidos");
        }

        [HttpPost]
        public ActionResult FinalizarPedido(int pedidoId)
        {
            _pedidoServicio.FinalizarPedido(pedidoId);
            return RedirectToAction("Pedidos");
        }


        [HttpGet]
        public ActionResult EditarPedido(int id, string estado)
        {
            Pedido pedido = _pedidoServicio.ObtenerPedidoPorId(id);

            if (estado != "Cerrado")
            {
                List<Usuario> invitados = _usuarioServicio.GetAll();
                List<GustoEmpanada> gustos = _pedidoServicio.GetGustoEmpanadas();
                ViewBag.invitados = invitados;
                ViewBag.gustos = gustos;

                return View("editar", pedido);
            }
            else
            {
                return RedirectToAction("Detalle", pedido);
            }
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]  //Para prevenir ataques CSRF
        public ActionResult EditarPedido(Pedido pedido)
        {

            pedido = _pedidoServicio.ObtenerPedidoPorId(pedido.IdPedido);

            int IdEnvioMail=0;

            int.TryParse(Request["enviarInvitacion"],out  IdEnvioMail);
                                                                       

            //si eligio invitados y gustos sigo
            if (ModelState.IsValid && Request["SelecInvitados"] != null && Request["SelecGustos"] != null)
            {
                //solo editar el pedido
               Pedido nuevo = _pedidoServicio.EditarPedidoExistente(pedido);

                _pedidoServicio.EliminarInvitacionesporPedido(pedido);
                _pedidoServicio.EliminarGustosporPedido(pedido);                
              
                _pedidoServicio.SetInvitados(nuevo, Request["SelecInvitados"].Split(','));
                                
                _pedidoServicio.SetGustos(nuevo, Request["SelecGustos"].Split(','));

                if (IdEnvioMail > 1)
                {
                    List<Usuario> usuariosAEnviarInvitacion = _pedidoServicio.DeterminarEnviosDeInvitacionSegunEstado(IdEnvioMail, pedido);
                    _pedidoServicio.EnviarMail(usuariosAEnviarInvitacion, pedido);
                }

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

        public ActionResult ElegirGustos(int id)
        {
            int usuarioLoguedo;
            Pedido pedido = _pedidoServicio.ObtenerPedidoPorId(id);

            usuarioLoguedo = ClsSesion.GetUsuarioLogueado().IdUsuario;

            ViewBag.IdUsuario = usuarioLoguedo;
            ViewBag.TokenInvitacion = pedido.InvitacionPedido.Where(i => i.IdPedido == pedido.IdPedido && i.IdUsuario == usuarioLoguedo).FirstOrDefault().Token.ToString();

            return View("elegir", pedido);

        }

        [HttpGet]
        public ActionResult Detalle(int idPedido)
        {

            Pedido detallepedido = new Pedido();

            detallepedido = _pedidoServicio.DetallePedido(idPedido);
            ViewBag.ListaDeUsuarios = _pedidoServicio.ObtenerUsuariosInvitadosDePedido(detallepedido);
            if (detallepedido.IdEstadoPedido == 2)
            {
                return View("Detalle", detallepedido);
            }
            else
            {
                return RedirectToAction("PedidoNoFinalizado");
            }
        }



        [HttpGet]
        public ActionResult PedidoNoFinalizado()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ElegirToken(System.Guid id)
        {

            Pedido pedido = _pedidoServicio.ObtenerPedidoByToken(id);

            if (pedido == null)
            {
                TempData["mensaje"] = "Identificacion invalida";
                return RedirectToAction("Error", "Home");
            }
            else if (pedido.IdEstadoPedido == 2)
            {
                TempData["mensaje"] = "No se puede elegir los gustos por que el pedido ya fue cerrado";
                return RedirectToAction("Error", "Home");
            }

            if (ClsSesion.GetUsuarioLogueado() == null)
            {
                return RedirectToAction("Login", "Home", new { redirigir = "/Pedidos/ElegirToken/" + id });
            }
            usuarioLogueado = ClsSesion.GetUsuarioLogueado();
            //Guid token = Guid.Parse(tokn);
           
            if (!(usuarioLogueado.IdUsuario == pedido.InvitacionPedido.Where(i => i.Token == id).Select(u => u.IdUsuario).FirstOrDefault()))
            {
                TempData["mensaje"] = "Acceso invalido";
                return RedirectToAction("Error", "Home");
            }
            if (!_pedidoServicio.InvitacionPedidoUsuarioIsTrue(pedido.IdPedido, usuarioLogueado))
            {
                TempData["mensaje"] = "Acceso invalido";
                return RedirectToAction("Error", "Home");
            }
            ViewBag.IdUsuario = usuarioLogueado.IdUsuario;
            ViewBag.TokenInvitacion = pedido.InvitacionPedido.Where(i => i.IdPedido == pedido.IdPedido && i.IdUsuario == usuarioLogueado.IdUsuario).FirstOrDefault().Token.ToString();
            return View("Elegir", pedido);
        }
    }
}
