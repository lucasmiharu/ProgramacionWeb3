using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProgramacionWeb3Tp.Models.Metadata
{
    public class PedidoMetadata
    {

        [Required(ErrorMessage = "El Nombre del Negocio es requerido")]
        [StringLength(200, ErrorMessage = "Debe tener como máximo 200 caracteres")]
        [Display(Name ="Comercio")]
        public string NombreNegocio { get; set; }

        [Required(ErrorMessage = "La descripcion es requerida")]
        [StringLength(1024, ErrorMessage = "Debe tener como máximo 1024 caracteres")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio unitario es requerido")]       
        [DataType(DataType.Currency)]
        [Display(Name = "Precio Unitario")]
        public int PrecioUnidad { get; set; }

        [Required(ErrorMessage = "El precio por unidad es requerido")]      
        [DataType(DataType.Currency)]
        [Display(Name = "Precio X Docena")]
        public int PrecioDocena { get; set; }  
       
    }
}