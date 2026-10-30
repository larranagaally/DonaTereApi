using DoñaTereApi.Entities;
using DoñaTereApi.Mappers;
using DoñaTereApi.Repositories;
using DoñaTere.Core.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DoñaTereApi.Services
{
    public class PedidoService
    {
        private readonly Repository<Pedido> pedidoRepository;
        private readonly Repository<Producto> productoRepository;

        public PedidoService(Repository<Pedido> pedidoRepository, Repository<Producto> productoRepository)
        {
            this.pedidoRepository = pedidoRepository;
            this.productoRepository = productoRepository;
        }

        public List<PedidoDTO> GetAll()
        {
            var pedidos = pedidoRepository.Query()
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.Pedidodetalles)
                    .ThenInclude(d => d.IdProductoNavigation)
                .ToList();

            return pedidos.Select(PedidoMapper.ToDto).ToList();
        }

        public PedidoDTO? GetById(int id)
        {
            var pedido = pedidoRepository.Query()
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.Pedidodetalles)
                    .ThenInclude(d => d.IdProductoNavigation)
                .FirstOrDefault(p => p.Id == id);

            return pedido == null ? null : PedidoMapper.ToDto(pedido);
        }

        public PedidoDTO Create(PedidoCreateDTO model)
        {
            
            if (model.FechaEntrega.Date <= DateTime.Today)
            {
                throw new InvalidOperationException("No se pueden crear pedidos para el mismo día. La fecha de entrega debe ser posterior a hoy.");
            }

            if (model.Detalles == null || !model.Detalles.Any())
            {
                throw new InvalidOperationException("El pedido debe contener al menos un producto.");
            }

            decimal totalCalculado = 0;

            
            var pedidoEntity = new Pedido
            {
                IdCliente = model.IdCliente,
                FechaEntrega = model.FechaEntrega,
                Pagado = false,
                Estado = "Pendiente",
                Pedidodetalles = new List<Pedidodetalles>()
            };

            foreach (var item in model.Detalles)
            {
                var producto = productoRepository.GetById(item.IdProducto);
                if (producto == null)
                {
                    throw new InvalidOperationException($"El producto con ID {item.IdProducto} no existe.");
                }

                if (!producto.Disponible)
                {
                    throw new InvalidOperationException($"El producto '{producto.Nombre}' no está disponible actualmente.");
                }

                decimal subtotal = producto.Precio * item.Cantidad;
                totalCalculado += subtotal;

                pedidoEntity.Pedidodetalles.Add(new Pedidodetalles
                {
                    IdProducto = item.IdProducto,
                    Cantidad = item.Cantidad,
                    Precio = producto.Precio
                });
            }

            pedidoEntity.Total = totalCalculado;

            pedidoRepository.Add(pedidoEntity);

           
            return GetById(pedidoEntity.Id)!;
        }

       
        public bool UpdateEstado(int id, string nuevoEstado)
        {
            var pedido = pedidoRepository.GetById(id);
            if (pedido == null) return false;

           
            if (nuevoEstado.ToLower() == "cancelado" && pedido.FechaEntrega.Date == DateTime.Today)
            {
                throw new InvalidOperationException("No se pueden cancelar pedidos el mismo día de la entrega.");
            }

            pedido.Estado = nuevoEstado;
            pedidoRepository.Update(pedido);
            return true;
        }

        
        public bool UpdatePago(int id, bool pagado)
        {
            var pedido = pedidoRepository.GetById(id);
            if (pedido == null) return false;

            pedido.Pagado = pagado;
            pedidoRepository.Update(pedido);
            return true;
        }


        public List<ReportePedidoPendienteDTO> GetPedidosPendientes(DateTime fecha)
        {
            var pedidos = pedidoRepository.Query()
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.Pedidodetalles)
                    .ThenInclude(d => d.IdProductoNavigation)
                .Where(p => p.FechaEntrega.Date == fecha.Date)
                .ToList();

            return pedidos.Select(ReporteMapper.ToDTO).ToList();
        }

        public List<ReporteProductoMasPedidoDTO> GetProductosMasPedidos()
        {
            var pedidos = pedidoRepository.Query()
                .Include(p => p.Pedidodetalles)
                    .ThenInclude(d => d.IdProductoNavigation)
                .ToList();

            var ranking = pedidos
                .SelectMany(p => p.Pedidodetalles)
                .GroupBy(d => d.IdProducto)
                .Select(g => new ReporteProductoMasPedidoDTO
                {
                    IdProducto = g.Key,
                    NombreProducto = g.FirstOrDefault()?.IdProductoNavigation?.Nombre ?? "Producto " + g.Key,
                    TotalPiezasVendidas = g.Sum(d => d.Cantidad)
                })
                .OrderByDescending(r => r.TotalPiezasVendidas)
                .ToList();

            return ranking;
        }

        public ReporteVentasMesDTO GetVentasDelMes(int año, int mes)
        {
            var pedidosMes = pedidoRepository.Query()
                .Where(p => p.FechaEntrega.Year == año && p.FechaEntrega.Month == mes)
                .ToList();

            return new ReporteVentasMesDTO
            {
                Año = año,
                Mes = mes,
                TotalVentas = pedidosMes.Sum(p => p.Total),
                CantidadPedidos = pedidosMes.Count
            };
        }


    }
}
