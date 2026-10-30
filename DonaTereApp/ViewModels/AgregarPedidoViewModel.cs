using DoñaTere.Core.DTOs;
using PROYECTOFINAL_API.Infrastructure;
using PROYECTOFINAL_API.Models;
using DonaTereApp.Services;

namespace PROYECTOFINAL_API.ViewModels
{
    public class AgregarPedidoViewModel : ViewModelBase
    {
        private readonly MainViewModel principal;
        private readonly ClientesService clientesService = new();
        private readonly ProductosService productosService = new();
        private readonly PedidosService pedidosService = new();

        private List<ClienteDTO> clientes = new();
        private List<LineaPedido> lineas = new();
        private ClienteDTO? clienteSeleccionado;
        private string mensaje = "";

        public AgregarPedidoViewModel(MainViewModel principal)
        {
            this.principal = principal;
            Pedido = new PedidoForm();

            OpcionesPago = new List<string> { "Pendiente", "Pagado" };
            OpcionesMetodo = new List<string> { "Efectivo", "Transferencia" };

            GuardarCommand = new RelayCommand(() => _ = GuardarAsync());
            VerPedidosCommand = new RelayCommand(principal.MostrarPedidos);

            _ = CargarAsync();
        }

        public string Titulo => "Nuevo pedido";

        public PedidoForm Pedido { get; }

        public DateTime FechaMinima => DateTime.Today.AddDays(1);

        public List<ClienteDTO> Clientes { get => clientes; private set => Asignar(ref clientes, value); }

        public ClienteDTO? ClienteSeleccionado
        {
            get => clienteSeleccionado;
            set => Asignar(ref clienteSeleccionado, value);
        }

        public List<LineaPedido> Lineas { get => lineas; private set => Asignar(ref lineas, value); }

        public List<string> OpcionesPago { get; }
        public List<string> OpcionesMetodo { get; }

        public decimal Total => Lineas.Sum(l => l.Subtotal);

        public string Mensaje { get => mensaje; set => Asignar(ref mensaje, value); }

        public RelayCommand GuardarCommand { get; }
        public RelayCommand VerPedidosCommand { get; }

        private async Task CargarAsync()
        {
            try
            {
                Clientes = await clientesService.GetAll();
                var productos = await productosService.GetAll();

                Lineas = productos
                    .Where(p => p.Disponible)
                    .OrderBy(p => p.CategoriaNombre)
                    .ThenBy(p => p.Nombre)
                    .Select(p => new LineaPedido(Productos.Desde(p), CalcularTotal))
                    .ToList();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }

        private void CalcularTotal() => OnPropertyChanged(nameof(Total));

        private async Task GuardarAsync()
        {
            Mensaje = "";

            if (ClienteSeleccionado == null)
            {
                Mensaje = "Seleccione el cliente.";
                return;
            }

            if (Pedido.FechaEntrega.Date < FechaMinima)
            {
                Mensaje = "Los pedidos se anotan con al menos un día de anticipación. Elija una fecha a partir de mañana.";
                return;
            }

            var seleccionadas = Lineas.Where(l => l.Cantidad > 0).ToList();
            if (seleccionadas.Count == 0)
            {
                Mensaje = "Agregue al menos un producto con cantidad mayor a cero.";
                return;
            }

            var dto = new PedidoCreateDTO
            {
                IdCliente = ClienteSeleccionado.Id,
                FechaEntrega = Pedido.FechaEntrega.Date,
                Detalles = seleccionadas.Select(l => new PedidoDetalleCreateDTO
                {
                    IdProducto = l.Producto.Id,
                    Cantidad = l.Cantidad
                }).ToList()
            };

            try
            {
                var creado = await pedidosService.Create(dto);

                if (Pedido.EstadoPago == "Pagado")
                {
                    await pedidosService.UpdatePago(creado.Id, true);
                }

                principal.MostrarPedidos();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}
