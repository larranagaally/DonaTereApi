using System.Collections.ObjectModel;
using DoñaTere.Core.DTOs;
using PROYECTOFINAL_API.Infrastructure;
using DonaTereApp.Services;

namespace PROYECTOFINAL_API.ViewModels
{
    public class ClientesViewModel : ViewModelBase
    {
        private readonly MainViewModel principal;
        private readonly ClientesService clientesService = new();
        private string mensaje = "";

        public ClientesViewModel(MainViewModel principal)
        {
            this.principal = principal;
            Clientes = new ObservableCollection<ClienteDTO>();

            NuevoCommand = new RelayCommand(principal.MostrarAgregarCliente);
            EditarCommand = new RelayCommand(c => principal.MostrarEditarCliente((ClienteDTO)c!));
            EliminarCommand = new RelayCommand(c => _ = EliminarAsync((ClienteDTO)c!));

            _ = CargarAsync();
        }

        public ObservableCollection<ClienteDTO> Clientes { get; }

        public string Mensaje { get => mensaje; set => Asignar(ref mensaje, value); }

        public RelayCommand NuevoCommand { get; }
        public RelayCommand EditarCommand { get; }
        public RelayCommand EliminarCommand { get; }

        public async Task CargarAsync()
        {
            try
            {
                var lista = await clientesService.GetAll();
                Clientes.Clear();
                foreach (var c in lista) Clientes.Add(c);
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }

        private async Task EliminarAsync(ClienteDTO cliente)
        {
            try
            {
                await clientesService.Delete(cliente.Id);
                Mensaje = "Cliente eliminado.";
                await CargarAsync();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}
