namespace ISO810_ComprasProject.Models
{
    public class TransaccionFiltroViewModel
    {
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public List<Transaccion> Transacciones { get; set; }
    }
}
