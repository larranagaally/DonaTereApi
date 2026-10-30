namespace DoñaTere.Core.DTOs
{
    public class PedidoCreateDTO
    {
        public int IdCliente { get; set; }
        public DateTime FechaEntrega { get; set; }
        public List<PedidoDetalleCreateDTO> Detalles { get; set; } = new();
    }
}
