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

        //Pantalla Login
        public ActionResult Login()
        {
            return View();
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


    }

}