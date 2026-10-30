using System.Collections.ObjectModel;
using PROYECTOFINAL_API.Infrastructure;
using PROYECTOFINAL_API.Models;
using DonaTereApp.Services;

namespace PROYECTOFINAL_API.ViewModels
{
    public class PedidosViewModel : ViewModelBase
    {
        private readonly MainViewModel principal;
        private readonly PedidosService pedidosService = new();
        private Pedidos? pedidoSeleccionado;
        private string mensaje = "";

        public PedidosViewModel(MainViewModel principal)
        {
            this.principal = principal;
            Pedidos = new ObservableCollection<Pedidos>();

            AgregarPedidoCommand = new RelayCommand(principal.MostrarAgregarPedido);
            EditarCommand = new RelayCommand(_ => Mensaje = "Editar pedidos todavía no está disponible en el servidor.");
            PagarEfectivoCommand = new RelayCommand(() => _ = EjecutarAsync(id => pedidosService.UpdatePago(id, true), "Pago registrado (efectivo)."));
            PagarTransferenciaCommand = new RelayCommand(() => _ = EjecutarAsync(id => pedidosService.UpdatePago(id, true), "Pago registrado (transferencia)."));
            EntregadoCommand = new RelayCommand(() => _ = EjecutarAsync(id => pedidosService.UpdateEstado(id, "Entregado"), "Pedido entregado."));
            CancelarCommand = new RelayCommand(() => _ = EjecutarAsync(id => pedidosService.UpdateEstado(id, "Cancelado"), "Pedido cancelado."));

            _ = CargarAsync(null);
        }

        public ObservableCollection<Pedidos> Pedidos { get; }

        public Pedidos? PedidoSeleccionado
        {
            get => pedidoSeleccionado;
            set
            {
                if (Asignar(ref pedidoSeleccionado, value))
                {
                    Mensaje = "";
                }
            }
        }

        public string Mensaje { get => mensaje; set => Asignar(ref mensaje, value); }

        public RelayCommand AgregarPedidoCommand { get; }
        public RelayCommand EditarCommand { get; }
        public RelayCommand PagarEfectivoCommand { get; }
        public RelayCommand PagarTransferenciaCommand { get; }
        public RelayCommand EntregadoCommand { get; }
        public RelayCommand CancelarCommand { get; }

        public async Task CargarAsync(int? seleccionarId)
        {
            try
            {
                var lista = (await pedidosService.GetAll()).Select(PROYECTOFINAL_API.Models.Pedidos.Desde)
                    .OrderBy(p => p.FechaEntrega)
                    .ToList();

                Pedidos.Clear();
                foreach (var p in lista) Pedidos.Add(p);

                PedidoSeleccionado = seleccionarId.HasValue
                    ? Pedidos.FirstOrDefault(p => p.Id == seleccionarId.Value)
                    : null;
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }

        private async Task EjecutarAsync(Func<int, Task> accion, string exito)
        {
            if (PedidoSeleccionado == null)
            {
                Mensaje = "Primero seleccione un pedido.";
                return;
            }

            var id = PedidoSeleccionado.Id;

            try
            {
                await accion(id);
                await CargarAsync(id);
                Mensaje = exito;
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}
