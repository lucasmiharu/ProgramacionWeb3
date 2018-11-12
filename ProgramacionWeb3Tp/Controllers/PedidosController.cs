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

        [HttpGet]
        public ActionResult iniciar()
        {
            Pedido pedido = new Pedido();

            return View(pedido);
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]  //Para prevenir ataques CSRF
        public ActionResult RegistrarPedido(Pedido pedido,Usuario usuario)
        {

            if (ModelState.IsValid)
            {

                _pedidoServicio.CrearPedido(pedido,usuario);
                return View("iniciado");

            }
            return View("iniciado");

        }

    }
}