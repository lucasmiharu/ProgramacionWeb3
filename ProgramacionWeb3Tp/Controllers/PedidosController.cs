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

            int envioInvitacion = _pedidoServicio.ObtenerEnviarInvitacion(pedido);



            /*Usuario us1 = new Usuario();
            us1.IdUsuario = 5;
            us1.Email = "ramitasilva@gmail.com";
            us1.Password = "rama";
            
            Usuario us2 = new Usuario();
            us2.IdUsuario = 4;
            us2.Email = "lucasmiharu@gmail.com";
            us2.Password = "lucas";



            List<Usuario> usuariosAEnviarInvitacion = new List<Usuario>();
            usuariosAEnviarInvitacion.Add(us1);
            usuariosAEnviarInvitacion.Add(us2);
            */
            List<Usuario> usuariosAEnviarInvitacion = _pedidoServicio.ObtenerUsuariosInvitadosDePedido(pedido);

            _pedidoServicio.EnviarMail(usuariosAEnviarInvitacion, pedido);
            

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

        public ActionResult ElegirToken(System.Guid id)
        {
            if (Session["usuario"] == null)
            {
                return RedirectToAction("Login", "Home", new { redirigir = "/Pedidos/ElegirToken/" + id });
            }
            usuarioLogueado = _pedidoServicio.BuscarUsuarioById(Convert.ToInt32(Session["usuario"]));
            //Guid token = Guid.Parse(tokn);
            Pedido pedido = _pedidoServicio.ObtenerPedidoByToken(id);
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
