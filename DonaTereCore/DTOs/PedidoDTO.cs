namespace DoñaTere.Core.DTOs
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public DateTime FechaEntrega { get; set; }
        public decimal Total { get; set; }
        public bool Pagado { get; set; }
        public string Estado { get; set; } = string.Empty; 
        public List<PedidoDetalleDTO> Detalle { get; set; } = new();
    }
}
