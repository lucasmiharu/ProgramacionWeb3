using ProgramacionWeb3Tp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ProgramacionWeb3Tp.Servicios
{
    public static class EmailService
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


        public static  void EnviarEmailInvitados(InvitacionPedido invitacion)
        {
                                   

            MailMessage email = new MailMessage("empanadadelaabuelaprogra@gmail.com", invitacion.Usuario.Email);
            //MailMessage email = new MailMessage("ramitasilva@gmail.com", invitacion.Usuario.Email);

            email.Subject = "Te invitó " + invitacion.Pedido.Usuario.Email + " para pedir empanadas";

            

            //email.Body = "Has sido invitado para realizar un pedido de empanadas http://localhost:59120/Pedidos/ElegirToken/" + invitacion.Token;
            email.Body = "<html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\"><head>  <title>Empanadas de la abuela</title> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0 \"> <meta name=\"format-detection\" content=\"telephone=no\"> <link href=\"https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700,800\" rel=\"stylesheet\"> <style type=\"text/css\"> body { 	margin: 0 !important; 	padding: 0 !important; 	-webkit-text-size-adjust: 100% !important; 	-ms-text-size-adjust: 100% !important; 	-webkit-font-smoothing: antialiased !important; } img { 	border: 0 !important; 	outline: none !important; } p { 	Margin: 0px !important; 	Padding: 0px !important; } table { 	border-collapse: collapse; 	mso-table-lspace: 0px; 	mso-table-rspace: 0px; } td, a, span { 	border-collapse: collapse; 	mso-line-height-rule: exactly; } .ExternalClass * { 	line-height: 100%; } .em_defaultlink a { 	color: inherit !important; 	text-decoration: none !important; } span.MsoHyperlink { 	mso-style-priority: 99; 	color: inherit; } span.MsoHyperlinkFollowed { 	mso-style-priority: 99; 	color: inherit; }  @media only screen and (min-width:481px) and (max-width:699px) { .em_main_table { 	width: 100% !important; } .em_wrapper { 	width: 100% !important; } .em_hide { 	display: none !important; } .em_img { 	width: 100% !important; 	height: auto !important; } .em_h20 { 	height: 20px !important; } .em_padd { 	padding: 20px 10px !important; } } @media screen and (max-width: 480px) { .em_main_table { 	width: 100% !important; } .em_wrapper { 	width: 100% !important; } .em_hide { 	display: none !important; } .em_img { 	width: 100% !important; 	height: auto !important; } .em_h20 { 	height: 20px !important; } .em_padd { 	padding: 20px 10px !important; } .em_text1 { 	font-size: 16px !important; 	line-height: 24px !important; } u + .em_body .em_full_wrap { 	width: 100% !important; 	width: 100vw !important; } } </style> </head>  <body class=\"em_body\" style=\"margin:0px; padding:0px;\" bgcolor=\"#efefef\"> <table class=\"em_full_wrap\" valign=\"top\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" bgcolor=\"#efefef\" align=\"center\">   <tbody><tr>     <td valign=\"top\" align=\"center\"><table class=\"em_main_table\" style=\"width:700px;\" width=\"700\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\">         <tbody>         <tr>           <td valign=\"top\" align=\"center\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\">               <tbody><tr>                 <td valign=\"top\" align=\"center\"><img class=\"em_img\" alt=\"merry Christmas\" style=\"display:block; font-family:Arial, sans-serif; font-size:30px; line-height:34px; color:#000000; max-width:700px;\" src=\"https://github.com/lucasmiharu/ProgramacionWeb3/blob/master/ProgramacionWeb3Tp/images/fondo.jpg?raw=true\" width=\"700\" border=\"0\" height=\"345\"></td>               </tr>             </tbody></table></td>         </tr>                  <tr>           <td style=\"padding:35px 70px 30px;\" class=\"em_padd\" valign=\"top\" bgcolor=\"#0d1121\" align=\"center\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\">               <tbody><tr>                 <td style=\"font-family:'Open Sans', Arial, sans-serif; font-size:16px; line-height:30px; color:#ffffff;\" valign=\"top\" align=\"center\">El usuario //ACA VA LA VARIABLE DEL CORREO// te invito a completar su pedido.</td>               </tr>               <tr>                 <td style=\"font-size:0px; line-height:0px; height:15px;\" height=\"15\">&nbsp;</td>               </tr>               <tr>                 <td style=\"font-family:'Open Sans', Arial, sans-serif; font-size:18px; line-height:22px; color:#fbeb59; letter-spacing:2px; padding-bottom:12px;\" valign=\"top\" align=\"center\">Ingresa y completa el pedido para poder continuar!</td>               </tr>               <tr>                 <td class=\"em_h20\" style=\"font-size:0px; line-height:0px; height:25px;\" height=\"25\">&nbsp;</td>               </tr> <tr>                 <td style=\"font-family:'Open Sans', Arial, sans-serif; font-size:18px; line-height:22px; color:#fbeb59; text-transform:uppercase; letter-spacing:2px; padding-bottom:12px;\" valign=\"top\" align=\"center\"><a  href=\"//ACA VA LA DIRECCION //\"><button class=\"btn btn-warning btn-lg\">Continuar al pedido</button></a></td>               </tr>             </tbody></table></td>         </tr>          <tr>           <td style=\"padding:38px 30px;\" class=\"em_padd\" valign=\"top\" bgcolor=\"#f6f7f8\" align=\"center\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\">               <tbody><tr>                               </tr>               <tr>                  © 2018 Empanadas de la abuela.Derechos reservados.<br>                </tr>             </tbody></table></td>         </tr>               </tbody></table></td>   </tr> </tbody></table> <div class=\"em_hide\" style=\"white-space: nowrap; display: none; font-size:0px; line-height:0px;\">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</div> </body></html>"
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
