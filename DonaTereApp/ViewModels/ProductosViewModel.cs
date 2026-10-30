using PROYECTOFINAL_API.Infrastructure;
using PROYECTOFINAL_API.Models;
using DonaTereApp.Services;

namespace PROYECTOFINAL_API.ViewModels
{
    public class ProductosViewModel : ViewModelBase
    {
        private readonly MainViewModel principal;
        private readonly ProductosService productosService = new();
        private List<GrupoProductos> grupos = new();
        private string mensaje = "";

        public ProductosViewModel(MainViewModel principal)
        {
            this.principal = principal;

            AgregarProductoCommand = new RelayCommand(principal.MostrarAgregarProducto);
            EditarCommand = new RelayCommand(p => principal.MostrarEditarProducto((Productos)p!));
            EliminarCommand = new RelayCommand(p => _ = EliminarAsync((Productos)p!));

            _ = CargarAsync();
        }

        public List<GrupoProductos> Grupos { get => grupos; private set => Asignar(ref grupos, value); }

        public string Mensaje { get => mensaje; set => Asignar(ref mensaje, value); }

        public RelayCommand AgregarProductoCommand { get; }
        public RelayCommand EditarCommand { get; }
        public RelayCommand EliminarCommand { get; }

        public async Task CargarAsync()
        {
            try
            {
                var productos = (await productosService.GetAll()).Select(Productos.Desde).ToList();
                Grupos = productos
                    .GroupBy(p => p.NombreCategoria)
                    .Select(g => new GrupoProductos(g.Key, g))
                    .ToList();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }

        private async Task EliminarAsync(Productos producto)
        {
            try
            {
                await productosService.Delete(producto.Id);
                Mensaje = "Producto eliminado.";
                await CargarAsync();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}
