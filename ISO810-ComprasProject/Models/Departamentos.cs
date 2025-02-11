
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ISO810_ComprasProject.Models
{
    public class Departamentos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartamentoId { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [StringLength(14)]
        public string Nombre { get; set; }

        public bool Activo { get; set; }


    }

}