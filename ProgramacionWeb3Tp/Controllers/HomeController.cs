using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProgramacionWeb3Tp.Models;
using ProgramacionWeb3Tp.Servicios;


namespace Pedido_Empanadas.Controllers
{
   
    public class HomeController : Controller
    {

        
        private readonly UsuarioServicio _usuarioServicio = new UsuarioServicio();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Error()
        {
            
            return View();
        }

        public ActionResult Productos()
        {
            return View();
        }

        public ActionResult Nosotros()
        {
            return View();
        }

        //Pantalla Login
        public ActionResult Login()
        {
            Usuario usr = new Usuario();
            return View(usr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  
        public ActionResult Loguear(Usuario usr)
        {
            Usuario usrLogueado;

            if (ModelState.IsValid)
            {
                usrLogueado =_usuarioServicio.LoguearUsuario(usr);

                if (usrLogueado == null)
                {
                    ViewBag.Mensaje = _usuarioServicio.mostrarMensajeDeError();
                    return View("Login",usr);
                }
                else
                {
                //agregar redirect to action a HomeUsuario
                    ViewBag.Mensaje = "Usuario logueado:" + usrLogueado.Email;
                    ViewBag.Usuario = usrLogueado.Email;
                    TempData["usuario"] = usrLogueado.Email;
                    Session["usuario"] = usrLogueado.Email;
                ClsSesion.SetUsuarioLogueado(usrLogueado);

                //Para testear la clase session 
                //ClsSesion.GetUsuarioLogueado();
                //ClsSesion.EliminarSesion();

                return RedirectToAction("Pedidos","Pedidos");
                }
            }
            else { return View("Login", usr); }

        }

        [HttpGet]
        public ActionResult Registracion()
        {
            Usuario usr = new Usuario();

            return View(usr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  //Para prevenir ataques CSRF
        public ActionResult RegistrarUsuario(Usuario usr)
        {
           
            if (ModelState.IsValid)
            {
                String pass2 = Request["pass2"];

              if ( _usuarioServicio.SaveUsuario(usr, pass2))
                {
                    ViewBag.Mensaje = "Usuario Generado con exito";                    
                }
              else
                {
                    ViewBag.Mensaje = _usuarioServicio.mostrarMensajeDeError();                    
                }
                                               
            }
           
            return View("Registracion");
        }

        [HttpGet]
        public ActionResult Salir()
        {

          if ( ClsSesion.VerificarLogueo(ClsSesion.GetUsuarioLogueado()))
            {
                ClsSesion.EliminarSesion();
                Session["Usuario"] = null;
            }

            return RedirectToAction("Index");
        }

    }

}