using DoñaTere.Core.DTOs;
using DoñaTereApi.Entities;

namespace DoñaTereApi.Mappers;

public static class ReporteMapper
{
    public static ReportePedidoPendienteDTO ToDTO(Pedido entity)
    {
        return new ReportePedidoPendienteDTO
        {
            Id = entity.Id,
            Cliente = entity.IdClienteNavigation?.Nombre ?? "Cliente " + entity.IdCliente,
            FechaEntrega = entity.FechaEntrega,
            Total = entity.Total,
            Pagado = entity.Pagado,
            Detalles = entity.Pedidodetalles?.Select(d => new DetalleReporteDTO
            {
                Producto = d.IdProductoNavigation?.Nombre ?? "Producto General",
                Cantidad = d.Cantidad,
                Subtotal = d.Cantidad * d.Precio
            }).ToList() ?? new()
        };
    }
}