using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ISO810_ComprasProject.Models
{
    public class UnidadesMedida
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UnidadMedidaId { get; set; }


        [Display(Name = "Descripcion")]
        [StringLength(50)]
        [Required(ErrorMessage = "El campo descripción es requerido")]
        public string Descripcion { get; set; }
                
        public bool Activo { get; set; }


    }

}