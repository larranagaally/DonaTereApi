using DoñaTereApi.Entities;
using DoñaTere.Core.DTOs;

namespace DoñaTereApi.Mappers
{
    public static class PedidoMapper
    {
        public static PedidoDTO ToDto(Pedido entidad)
        {
            return new PedidoDTO
            {
                Id = entidad.Id,
                IdCliente = entidad.IdCliente,
                ClienteNombre = entidad.IdClienteNavigation?.Nombre ?? string.Empty,
                FechaEntrega = entidad.FechaEntrega,
                Total = entidad.Total,
                Pagado = entidad.Pagado,
                Estado = entidad.Estado,
                Detalle = entidad.Pedidodetalles?.Select(d => new PedidoDetalleDTO
                {
                    IdProducto = d.IdProducto,
                    ProductoNombre = d.IdProductoNavigation?.Nombre ?? string.Empty,
                    Cantidad = d.Cantidad,
                    Precio = d.Precio
                }).ToList() ?? new()
            };
        }

        public static Pedido ToEntity(PedidoCreateDTO dto, decimal totalCalculado)
        {
            return new Pedido
            {
                IdCliente = dto.IdCliente,
                FechaEntrega = dto.FechaEntrega,
                Total = totalCalculado,
                Pagado = false,
                Estado = "Pendiente", 
                Pedidodetalles = dto.Detalles.Select(d => new Pedidodetalles
                {
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad,
                   
                }).ToList()
            };
        }
    }
}



