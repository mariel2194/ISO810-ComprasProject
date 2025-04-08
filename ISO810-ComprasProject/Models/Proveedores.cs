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

        [Display(Name = "Cédula")]
        [StringLength(14)]
        public string Cedula { get; set; }

        [Display(Name = "RNC")]
        [StringLength(11)]
        public string RNC { get; set; }

        [Display(Name = "Tipo de Documento")]
        public string TipoDocumento { get; set; }

        [Required(ErrorMessage = "El nombre comercial es requerido.")]
        [Display(Name = "Nombre Comercial")]
        [StringLength(25)]
        public string NombreComercial { get; set; }

        public bool Activo { get; set; }
    }

}


