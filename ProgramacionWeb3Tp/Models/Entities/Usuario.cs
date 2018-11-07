using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using ProgramacionWeb3Tp.Models.Metadata;


namespace ProgramacionWeb3Tp.Models
{
    [MetadataType(typeof(UsuarioMetadata))]
    public partial class Usuario
    {
       
        public Usuario(Usuario usuario)
        {           
            this.Email = usuario.Email;
            this.Password = usuario.Password;           
        }
                      
              
    }
}