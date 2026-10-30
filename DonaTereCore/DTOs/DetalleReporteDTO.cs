namespace DoñaTere.Core.DTOs
{
    public class DetalleReporteDTO
    {
        public string Producto { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}
