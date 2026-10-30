using System.Collections.ObjectModel;
using System.Globalization;
using PROYECTOFINAL_API.Infrastructure;
using PROYECTOFINAL_API.Models;
using DonaTereApp.Services;

namespace PROYECTOFINAL_API.ViewModels
{
    public class ReportesViewModel : ViewModelBase
    {
        private readonly PedidosService pedidosService = new();

        private List<Pedidos> todos = new();
        private DateTime fechaCocina = DateTime.Today.AddDays(1);
        private DateTime mes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        private int pestana;
        private string mensaje = "";

        public ReportesViewModel()
        {
            PedidosDelDia = new ObservableCollection<Pedidos>();
            ProduccionDelDia = new ObservableCollection<ResumenProducto>();
            ProductosMasPedidos = new ObservableCollection<ResumenProducto>();

            MesAnteriorCommand = new RelayCommand(() => Mes = Mes.AddMonths(-1));
            MesSiguienteCommand = new RelayCommand(() => Mes = Mes.AddMonths(1));
            VerQueCocinarCommand = new RelayCommand(() => Pestana = 0);
            VerMasPedidosCommand = new RelayCommand(() => Pestana = 1);
            VerVentasMesCommand = new RelayCommand(() => Pestana = 2);

            _ = CargarAsync();
        }

        public string Mensaje { get => mensaje; set => Asignar(ref mensaje, value); }

        public int Pestana
        {
            get => pestana;
            private set
            {
                if (Asignar(ref pestana, value))
                {
                    OnPropertyChanged(nameof(EsQueCocinar));
                    OnPropertyChanged(nameof(EsMasPedidos));
                    OnPropertyChanged(nameof(EsVentasMes));
                }
            }
        }

        public bool EsQueCocinar => Pestana == 0;
        public bool EsMasPedidos => Pestana == 1;
        public bool EsVentasMes => Pestana == 2;

        public RelayCommand VerQueCocinarCommand { get; }
        public RelayCommand VerMasPedidosCommand { get; }
        public RelayCommand VerVentasMesCommand { get; }
        public RelayCommand MesAnteriorCommand { get; }
        public RelayCommand MesSiguienteCommand { get; }

        public DateTime FechaCocina
        {
            get => fechaCocina;
            set
            {
                if (Asignar(ref fechaCocina, value.Date))
                {
                    ActualizarCocina();
                }
            }
        }

        public ObservableCollection<Pedidos> PedidosDelDia { get; }
        public ObservableCollection<ResumenProducto> ProduccionDelDia { get; }
        public ObservableCollection<ResumenProducto> ProductosMasPedidos { get; }

        public DateTime Mes
        {
            get => mes;
            private set
            {
                if (Asignar(ref mes, value))
                {
                    OnPropertyChanged(nameof(MesTexto));
                    OnPropertyChanged(nameof(VentasMes));
                    OnPropertyChanged(nameof(CobradoMes));
                    OnPropertyChanged(nameof(PorCobrarMes));
                    OnPropertyChanged(nameof(NumeroPedidosMes));
                }
            }
        }

        public string MesTexto
        {
            get
            {
                string texto = Mes.ToString("MMMM yyyy", new CultureInfo("es-MX"));
                return char.ToUpper(texto[0]) + texto.Substring(1);
            }
        }

        private List<Pedidos> EntregasDelMes() =>
            todos.Where(p => p.Estado != "Cancelado"
                && p.FechaEntrega.Year == Mes.Year
                && p.FechaEntrega.Month == Mes.Month).ToList();

        public decimal VentasMes => EntregasDelMes().Sum(p => p.Total);
        public decimal CobradoMes => EntregasDelMes().Where(p => p.Pagado).Sum(p => p.Total);
        public decimal PorCobrarMes => VentasMes - CobradoMes;
        public int NumeroPedidosMes => EntregasDelMes().Count;

        public async Task CargarAsync()
        {
            try
            {
                todos = (await pedidosService.GetAll()).Select(Pedidos.Desde).ToList();

                ActualizarCocina();

                ProductosMasPedidos.Clear();
                foreach (var r in Agrupar(todos.Where(p => p.Estado != "Cancelado"))
                    .OrderByDescending(r => r.Piezas))
                {
                    ProductosMasPedidos.Add(r);
                }

                OnPropertyChanged(nameof(VentasMes));
                OnPropertyChanged(nameof(CobradoMes));
                OnPropertyChanged(nameof(PorCobrarMes));
                OnPropertyChanged(nameof(NumeroPedidosMes));
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }

        private void ActualizarCocina()
        {
            PedidosDelDia.Clear();
            foreach (var p in todos
                .Where(p => p.Estado == "Pendiente" && p.FechaEntrega.Date == FechaCocina)
                .OrderBy(p => p.NombreCliente))
            {
                PedidosDelDia.Add(p);
            }

            ProduccionDelDia.Clear();
            foreach (var r in Agrupar(PedidosDelDia)) ProduccionDelDia.Add(r);
        }

        private static List<ResumenProducto> Agrupar(IEnumerable<Pedidos> pedidos)
        {
            return pedidos
                .SelectMany(p => p.Detalles)
                .GroupBy(d => d.NombreProducto)
                .Select(g => new ResumenProducto
                {
                    NombreProducto = g.Key,
                    Piezas = g.Sum(d => d.Cantidad),
                    Ingreso = g.Sum(d => d.Subtotal)
                })
                .OrderBy(r => r.NombreProducto)
                .ToList();
        }
    }
}
