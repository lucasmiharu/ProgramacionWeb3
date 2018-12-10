using ProgramacionWeb3Tp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ProgramacionWeb3Tp.Servicios
{
    public class EmailService
    {
        /*
        public void EnviarEmailResponsable(Pedido pedido)
        {
            //MailMessage email = new MailMessage("3empanadaspw3@gmail.com", "ramitasilva@gmail.com");
            MailMessage email = new MailMessage("ramitasilva@gmail.com", pedido.Usuario.Email);
            email.Subject = pedido.Usuario.Email + " sos el responsable de este pedido: " + pedido.NombreNegocio.ToString();

            string listaInvitados = "";
            string listaGustos = "";

            foreach (var invitado in info.Invitados)
            {
                string inv = invitado.Email + " : $" + invitado.Precio.ToString() + " <br>";
                listaInvitados += inv;
            }

            foreach (var gusto in pedido.)
            {
                string gus = gusto.Gusto + " : " + gusto.Cantidad.ToString() + "<br>";
                listaGustos += gus;
            }

            email.Body = "<h2>Precio Total: $" + info.PrecioTotal.ToString() + "</h2>" +
                        "<h2>Detalle Recaudacion:</h2>" +
                        "<div>" +
                        "  <h3>Invitados:</h3>" +
                        "  <div>" +
                        listaInvitados +
                        "  </div>" +
                        "</div>" +
                        "<br>" +
                        "<div >" +
                        "  <h3>Detalle Pedido:</h3>" +
                        "  <div>" +
                        listaGustos +
                         " </div>" +
                        "</div>";

            email.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Send(email);
        }
        */


        public void EnviarEmailInvitados(InvitacionPedido invitacion)
        {
           
            MailMessage email = new MailMessage("lucasmiharu@gmail.com", "lucasmiharu@gmail.com");
            //MailMessage email = new MailMessage("ramitasilva@gmail.com", invitacion.Usuario.Email);

            email.Subject = "Te invitó " + invitacion.Pedido.Usuario.Email + " para pedir empanadas";            
            email.Body = "Has sido invitado para realizar un pedido de empanadas http://localhost:50846/Pedidos/ElegirToken/" + invitacion.Token;

            //defino valores del SMTP
            SmtpClient smtp = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 25,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("empanadadelaabuelaprogra@gmail.com", "empanadadelaabuelaprogra123"),
                Timeout = 20000
            };
                   
             smtp.Send(email);
            
        }
    }
}