using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ISO810_ComprasProject.Models
{
    public class Articulos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArticuloId { get; set; }

        [Required(ErrorMessage = "El campo descripción es requerido")]
        [Display(Name = "Descripción")]
        [StringLength(15, ErrorMessage = "La descripción no debe exceder de 15 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo Marca es requerido")]
        [Display(Name = "Marca")]
        [StringLength(15, ErrorMessage = "La marca no debe exceder de 15 caracteres")]
        public string Marca { get; set; }

        public int UnidadMedidaId { get; set; }

        [Display(Name = "Unidad de Medida")]
        [ForeignKey("UnidadMedidaId")]
        [ValidateNever]
        public virtual UnidadesMedida UnidadMedida { get; set; }

        [Required(ErrorMessage = "El costo unitario es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El costo debe ser mayor que cero.")]
        [Display(Name = "Costo Unitario")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoUnitario { get; set; }


        [Display(Name = "Stock")]
        [Required(ErrorMessage = "El stock unitario es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser mayor a cero.")]
        public int Stock { get; set; }

        [Required]
        public bool Activo { get; set; } = true;


    }

}