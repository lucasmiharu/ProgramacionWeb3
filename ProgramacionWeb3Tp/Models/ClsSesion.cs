using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.WebPages;

using System.Web.UI.WebControls;
using System.Configuration;


using System.Runtime.Remoting.Contexts;
using System.Web.Mvc;
using System.Net;
using System.Text;

namespace ProgramacionWeb3Tp.Models
{
    public static class ClsSesion
    {

        
        public static Usuario GetUsuarioLogueado()
        {
            Usuario usuario = new Usuario();


            if (HttpContext.Current != null &&
    HttpContext.Current.Session != null &&
    HttpContext.Current.Session["UserId"] != null)
            {
                usuario.IdUsuario = int.Parse((HttpContext.Current.Session["UserId"].ToString()));
                usuario.Email = (HttpContext.Current.Session["UserEmail"].ToString());
            }

        else
            { usuario = null; }
            
            return usuario;           
            
        }

        public static void SetUsuarioLogueado(Usuario usuario)
        {           

            if (HttpContext.Current != null &&
    HttpContext.Current.Session != null &&
    HttpContext.Current.Session["UserId"] == null)
            {
                HttpContext.Current.Session.Add("UserId", usuario.IdUsuario);
                HttpContext.Current.Session.Add("UserEmail", usuario.Email);                
            }
           
        }

        public static void EliminarSesion()
        {

            if (HttpContext.Current != null &&
    HttpContext.Current.Session != null &&
    HttpContext.Current.Session["UserId"] != null)
            {

                HttpContext.Current.Session.Remove("UserId");
                HttpContext.Current.Session.Remove("UserEmail");
            }
        }


    }
}