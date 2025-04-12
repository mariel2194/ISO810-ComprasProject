using System;
using System.ComponentModel.DataAnnotations;

namespace ISO810_ComprasProject.Models
{
    public class Transaccion
    {
        [Key]
        public int TransaccionId { get; set; } 

        [StringLength(200)]
        public string Descripcion { get; set; }

        public DateTime Fecha { get; set; }

        public decimal Monto { get; set; }

        public int? AsientoId { get; set; }

        public DateTime? FechaAsiento { get; set; }
    }
}
