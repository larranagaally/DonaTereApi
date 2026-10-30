using System.Collections.ObjectModel;
using PROYECTOFINAL_API.Infrastructure;
using PROYECTOFINAL_API.Models;
using DonaTereApp.Services;

namespace PROYECTOFINAL_API.ViewModels
{
    public class InicioViewModel : ViewModelBase
    {
        private readonly PedidosService pedidosService = new();
        private readonly ProductosService productosService = new();

        private int pedidosPendientes;
        private int entregasDelDia;
        private int totalProductos;
        private decimal porCobrar;

        public InicioViewModel()
        {
            ProximasEntregas = new ObservableCollection<Pedidos>();
            _ = CargarAsync();
        }

        public int PedidosPendientes { get => pedidosPendientes; private set => Asignar(ref pedidosPendientes, value); }
        public int EntregasDelDia { get => entregasDelDia; private set => Asignar(ref entregasDelDia, value); }
        public int TotalProductos { get => totalProductos; private set => Asignar(ref totalProductos, value); }
        public decimal PorCobrar { get => porCobrar; private set => Asignar(ref porCobrar, value); }

        public ObservableCollection<Pedidos> ProximasEntregas { get; }

        public async Task CargarAsync()
        {
            try
            {
                var pedidos = (await pedidosService.GetAll()).Select(Pedidos.Desde).ToList();
                var productos = await productosService.GetAll();
                var hoy = DateTime.Today;

                PedidosPendientes = pedidos.Count(p => p.Estado == "Pendiente");
                EntregasDelDia = pedidos.Count(p => p.Estado == "Pendiente" && p.FechaEntrega.Date == hoy);
                TotalProductos = productos.Count(p => p.Disponible);
                PorCobrar = pedidos.Where(p => p.Estado != "Cancelado" && !p.Pagado).Sum(p => p.Total);

                ProximasEntregas.Clear();
                foreach (var p in pedidos
                    .Where(p => p.Estado == "Pendiente" && p.FechaEntrega.Date >= hoy)
                    .OrderBy(p => p.FechaEntrega)
                    .Take(5))
                {
                    ProximasEntregas.Add(p);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Inicio: {ex.Message}");
            }
        }
    }
}
