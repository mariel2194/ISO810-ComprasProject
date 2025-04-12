using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ISO810_ComprasProject.Models
{
    public class OrdenCompras
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompraId { get; set; }

        [Required(ErrorMessage = "El campo Fecha es requerido")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        public int ArticuloId { get; set; }

        [ForeignKey("ArticuloId")]
        [Display(Name = "Artículo")]
        [ValidateNever]
        public virtual Articulos Articulo { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Cantidad debe ser mayor que 0.")]
        public int Cantidad { get; set; }  
        
        public int UnidadMedidaId { get; set; }

        [ForeignKey("UnidadMedidaId")]
        [Display(Name = "Unidad de Medida")]
        [ValidateNever]
        public virtual UnidadesMedida UnidadMedida { get; set; }


        public double Monto { get; set; }


        [Display(Name = "Estado")]
        public EstadoCompra Estado { get; set; } = EstadoCompra.Pendiente;

        public int? TransaccionId { get; set; }

        [ValidateNever]
        public virtual Transaccion Transaccion { get; set; }


    }

    public enum EstadoCompra
    {
        Pendiente,
        Aprobada,
        Rechazada,
        Completada
    }

}