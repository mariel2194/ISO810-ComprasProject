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

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        public int ArticuloId { get; set; }

        [ForeignKey("ArticuloId")]
        public virtual Articulos Articulo { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Cantidad debe ser mayor que 0.")]
        public int Cantidad { get; set; }        

        public int UnidadMedidaId { get; set; }

        [ForeignKey("UnidadMedidaId")]
        public virtual UnidadesMedida UnidadMedida { get; set; }

        public EstadoCompra Estado { get; set; } = EstadoCompra.Pendiente;


    }

    public enum EstadoCompra
    {
        Pendiente,
        Aprobada,
        Rechazada,
        Completada
    }

}