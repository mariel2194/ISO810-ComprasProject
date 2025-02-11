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

        [Required]
        [Display(Name = "Descripcion")]
        [StringLength(14)]        
        public  string Descripcion { get; set; }

        [Required]
        [Display(Name = "Marca")]
        [StringLength(15)]
        public string Marca { get; set; }

        public int UnidadMedidaId { get; set; }

        [ForeignKey("UnidadMedidaId")]
        public virtual UnidadesMedida UnidadMedida { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoUnitario { get; set; }

        [Display(Name = "Stock")]
        public int Stock { get; set; }

        [Required]
        public bool Activo { get; set; } = true;


    }

}