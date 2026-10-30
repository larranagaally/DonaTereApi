
namespace DoñaTere.Core.DTOs
{
    public class ReportePedidoPendienteDTO
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = null!;
        public DateTime FechaEntrega { get; set; }
        public decimal Total { get; set; }
        public bool Pagado { get; set; }
        public List<DetalleReporteDTO> Detalles { get; set; } = new();
    }
}
