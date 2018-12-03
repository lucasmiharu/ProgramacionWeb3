using ProgramacionWeb3Tp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ProgramacionWeb3Tp.Servicios
{
    public class UsuarioServicio
    {

        private Pedidos condb = new Pedidos();
              
        //Variables para mostrar los mensajes de error
        private static string MENSAJE_MAIL_PASS_INCORRECTOS = "E-mail y/o contraseña incorrectos, por favor intentá nuevamente";
        private static string MENSAJE_MAIL_EXISTENTE = "Ese e-mail no está disponible, por favor ingresá otro";
        private static string MENSAJE_ERROR_INESPERADO = "Ocurrio un error inesperado en la aplicación. Por favor, Intentelo más tarde";

        //Codigo de error a asignar - Registro
        private int codigoErrorRegistro = 0;

        public Usuario GetUsuario(int id)
        {

            var user = (from u in condb.Usuario
                        where u.IdUsuario == id
                        select u)
                        .FirstOrDefault();

            return user;
        }

        public List<Usuario> GetAll()
        {

            List <Usuario> users = (from u in condb.Usuario select u).ToList();

            return users;

        }

        public Usuario LoguearUsuario(Usuario usuario)
        {
            Usuario usuarioOK = new Usuario();

            usuarioOK = condb.Usuario.Where(u => u.Email == usuario.Email
                                               && u.Password == usuario.Password).SingleOrDefault();
            if (usuarioOK == null)
            {
                codigoErrorRegistro = 0;
            }

            return usuarioOK;
        }


        public Usuario GetUsuario(string email)
        {
            Usuario usuarioEncontrado = new Usuario();

            usuarioEncontrado = condb.Usuario.Where(u => u.Email == email).SingleOrDefault();

            return usuarioEncontrado;
        }


        public bool SaveUsuario(Usuario usuario, string pass2)
        {
            Usuario usuarioNuevo = null;
            bool ok  = false;
            if (chequearSiMailsCoinciden(usuario.Password, pass2))
            {
                usuarioNuevo = new Usuario(usuario);

               
                if (chequearSiExisteEmail(usuario.Email))
                {//el email ya esta registrado
                    codigoErrorRegistro = 1;
                    ok = false;
                }
                else
                {
                    try
                    {
                        condb.Usuario.Add(usuario);
                        condb.SaveChanges();
                        ok = true;

                    }
                    catch (DbEntityValidationException ex)
                    {
                        ok = false;
                        codigoErrorRegistro = 2;
                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                            {
                                System.Diagnostics.Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                            }
                        }
                    }
                }
            }
            else
            {
                //Para mostrar algo si no coinciden los emails
                codigoErrorRegistro = 0;
                ok = false;
            }

            return ok;
        }

        public bool chequearSiMailsCoinciden(string pass1, string pass2)
        {
            if (pass2.Equals(pass1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool chequearSiExisteEmail(string email)
        {
            //var usuario = context.Usuarios.Where(u => u.Email == email).Select(u1 => u1).First();
            Usuario usuario = (from u in condb.Usuario
                               where u.Email == email
                               select u).FirstOrDefault();
            if (usuario != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

      
        public void activarRegistroUsuarioExistente(Usuario usuario)
        {
            //var usuarioExistente = context.Usuarios.Where(u => u.IdUsuario == usuario.IdUsuario).First();
            Usuario usuarioExistente = (from u in condb.Usuario
                                        where u.Email == usuario.Email
                                        select u).First();
          
            usuarioExistente.Password = usuario.Password;                      
            condb.SaveChanges();

            //return usuarioExistente;
        }

        public String mostrarMensajeDeError()
        {
            string mensaje = "";
            if (codigoErrorRegistro == 0)
            {
                mensaje = MENSAJE_MAIL_PASS_INCORRECTOS;
            }
            else if (codigoErrorRegistro == 1)
            {
                mensaje = MENSAJE_MAIL_EXISTENTE;
            }
            else if (codigoErrorRegistro == 2)
            {
                mensaje = MENSAJE_ERROR_INESPERADO;
            }
            return mensaje;
        }

    }
}