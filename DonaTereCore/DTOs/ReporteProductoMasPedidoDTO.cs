namespace DoñaTere.Core.DTOs
{
    public class ReporteProductoMasPedidoDTO
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = null!;
        public int TotalPiezasVendidas { get; set; }
    }
}
