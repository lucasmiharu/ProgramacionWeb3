using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using appiPedidos.Models;
using ActionNameAttribute = System.Web.Http.ActionNameAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using appiPedidos.Servicios;
using Newtonsoft.Json;

namespace appiPedidos.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PedidoController : ApiController
    {        
        private static AppiServicio _apiSrv = new AppiServicio();
        
    
        // POST api/Pedido        
        public string PostPedido(UsuarioGustos GustosUsuario)
        {
            
            MensajeAppi msg = new MensajeAppi();

           msg= _apiSrv.ElegirGustos(GustosUsuario);
                                                   

            return JsonConvert.SerializeObject(msg);

        }

    }

}



