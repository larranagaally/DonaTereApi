using DoñaTere.Core.DTOs;

namespace PROYECTOFINAL_API.Models
{
    public class PedidoDetalle
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal PrecioUnidad { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnidad;
    }

  
    public class Pedidos
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } = "";
        public DateTime FechaEntrega { get; set; }
        public decimal Total { get; set; }
        public bool Pagado { get; set; }
        public string Estado { get; set; } = "";
        public List<PedidoDetalle> Detalles { get; set; } = new();

        public string EstadoPago => Pagado ? "Pagado" : "Pendiente";

        public string PagoTexto => Estado == "Cancelado" ? "—" : (Pagado ? "Pagado" : "Pendiente");

        public string ResumenProductos =>
            string.Join(", ", Detalles.Select(d => $"{d.Cantidad} × {d.NombreProducto}"));

        public static Pedidos Desde(PedidoDTO dto)
        {
            return new Pedidos
            {
                Id = dto.Id,
                IdCliente = dto.IdCliente,
                NombreCliente = dto.ClienteNombre,
                FechaEntrega = dto.FechaEntrega,
                Total = dto.Total,
                Pagado = dto.Pagado,
                Estado = dto.Estado,
                Detalles = dto.Detalle.Select(d => new PedidoDetalle
                {
                    IdProducto = d.IdProducto,
                    NombreProducto = d.ProductoNombre,
                    Cantidad = d.Cantidad,
                    PrecioUnidad = d.Precio
                }).ToList()
            };
        }
    }

    
    public class PedidoForm
    {
        public DateTime FechaEntrega { get; set; } = DateTime.Today.AddDays(1);
        public string EstadoPago { get; set; } = "Pendiente";
        public string MetodoPago { get; set; } = "";
    }
}
