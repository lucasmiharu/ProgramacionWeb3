using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProgramacionWeb3Tp.Models.Metadata
{
    public class UsuarioMetadata
    {
       
        [Required(ErrorMessage = "El Email es requerido")]
        [StringLength(50, ErrorMessage = "Debe tener como máximo 50 caracteres")]
        [EmailAddress(ErrorMessage = "Ingrese un e-mail válido")]
        //[CustomValidation(typeof(Usuario), "ValidarEmailUnico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio")]
        [StringLength(20, ErrorMessage = "Debe tener como máximo 20 caracteres")]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=\\w*\\d)(?=\\w*[A-Z])(?=\\w*[a-z])\\S{0,20}$", ErrorMessage = "Debe contener al menos 1 mayúscula, 1 minúscula y 1 número")]
        public string Password { get; set; }

        public static ValidationResult ValidarEmailUnico(object value, ValidationContext context)
        {
            var usuario = context.ObjectInstance as Usuario;

            if (usuario == null || string.IsNullOrEmpty(usuario.Email))
                return new ValidationResult(string.Format("Email es requerido."));

            //para validar que no exista otro email igual, debo chequear en la base
            Pedidos db = new Pedidos();

            var existeEmail = db.Usuario.Any((o => o.Email == usuario.Email));

            if (existeEmail)
            {
                return new ValidationResult(string.Format("El Email {0} ya está siendo usado.", usuario.Email));
            }



            return ValidationResult.Success;
        }


    }
}