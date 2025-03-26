using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ISO810_ComprasProject.Models
{
    public class Proveedores
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProveedorId { get; set; }

        [Required(ErrorMessage = "El campo  es requerido")]

        [Display(Name = "Cédula")]
        [StringLength(14)]
        public string Cedula { get; set; }

        [Display(Name = "RNC")]
        [StringLength(14)]
        public string RNC { get; set; }
        public String TipoDocumento { get; set; }

        [Required(ErrorMessage = "El nombre comercial es requerido.")]
        [Display(Name = "Nombre Comercial")]
        [StringLength(25)]
        public string NombreComercial { get; set; }

        public bool Activo { get; set; }


    }
   
    

}