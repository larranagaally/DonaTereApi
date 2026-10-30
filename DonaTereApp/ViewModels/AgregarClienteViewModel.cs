using System.Linq;
using DoñaTere.Core.DTOs;
using PROYECTOFINAL_API.Infrastructure;
using DonaTereApp.Services;

namespace PROYECTOFINAL_API.ViewModels
{
    public class AgregarClienteViewModel : ViewModelBase
    {
        private readonly MainViewModel principal;
        private readonly ClienteDTO? original;
        private readonly ClientesService clientesService = new();
        private string mensaje = "";

        public AgregarClienteViewModel(MainViewModel principal, ClienteDTO? original)
        {
            this.principal = principal;
            this.original = original;

            Cliente = new ClienteCreateDTO
            {
                Nombre = original?.Nombre ?? "",
                Telefono = original?.Telefono ?? ""
            };

            GuardarCommand = new RelayCommand(() => _ = GuardarAsync());
            VerClientesCommand = new RelayCommand(principal.MostrarClientes);
        }

        public string Titulo => original == null ? "Nuevo cliente" : "Editar cliente";

        public ClienteCreateDTO Cliente { get; }

        public string Mensaje { get => mensaje; set => Asignar(ref mensaje, value); }

        public RelayCommand GuardarCommand { get; }
        public RelayCommand VerClientesCommand { get; }

        private async Task GuardarAsync()
        {
            Mensaje = "";
            Cliente.Nombre = (Cliente.Nombre ?? "").Trim();
            Cliente.Telefono = (Cliente.Telefono ?? "").Trim();

            if (Cliente.Nombre.Length == 0)
            {
                Mensaje = "Escriba el nombre del cliente.";
                return;
            }

            if (Cliente.Telefono.Length != 10 || !Cliente.Telefono.All(char.IsDigit))
            {
                Mensaje = "El teléfono debe tener 10 dígitos, sin espacios ni guiones.";
                return;
            }

            try
            {
                if (original == null)
                {
                    await clientesService.Create(Cliente);
                }
                else
                {
                    await clientesService.Update(original.Id, Cliente);
                }

                principal.MostrarClientes();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}
