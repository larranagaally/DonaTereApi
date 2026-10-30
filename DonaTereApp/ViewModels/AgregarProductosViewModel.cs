using System.Collections.ObjectModel;
using System.Globalization;
using DoñaTere.Core.DTOs;
using PROYECTOFINAL_API.Infrastructure;
using PROYECTOFINAL_API.Models;
using DonaTereApp.Services;

namespace PROYECTOFINAL_API.ViewModels
{
    public class AgregarProductosViewModel : ViewModelBase
    {
        private readonly MainViewModel principal;
        private readonly Productos? original;
        private readonly CategoriasService categoriasService = new();
        private readonly ProductosService productosService = new();
        private ObservableCollection<CategoriaDTO> categorias = new();
        private string mensaje = "";

        public AgregarProductosViewModel(MainViewModel principal, Productos? original)
        {
            this.principal = principal;
            this.original = original;

            Producto = original == null
                ? new Productos { Activo = true, Disponible = true }
                : new Productos
                {
                    Id = original.Id,
                    Nombre = original.Nombre,
                    Precio = original.Precio,
                    Activo = true,
                    Disponible = original.Disponible,
                    CategoriaId = original.CategoriaId,
                    NombreCategoria = original.NombreCategoria
                };

            Precio = original == null ? "" : original.Precio.ToString("0.00", CultureInfo.InvariantCulture);

            GuardarCommand = new RelayCommand(() => _ = GuardarAsync());
            VerProductosCommand = new RelayCommand(principal.MostrarProductos);

            _ = CargarCategoriasAsync();
        }

        public string Titulo => original == null ? "Agregar producto" : "Editar producto";

        public Productos Producto { get; }

        public string Precio { get; set; } = "";

        public ObservableCollection<CategoriaDTO> Categorias
        {
            get => categorias;
            private set => Asignar(ref categorias, value);
        }

        public CategoriaDTO? CategoriaSeleccionada
        {
            get => Categorias.FirstOrDefault(c => c.Id == Producto.CategoriaId);
            set
            {
                Producto.CategoriaId = value?.Id ?? 0;
                OnPropertyChanged();
            }
        }

        public string Mensaje { get => mensaje; set => Asignar(ref mensaje, value); }

        public RelayCommand GuardarCommand { get; }
        public RelayCommand VerProductosCommand { get; }

        private async Task CargarCategoriasAsync()
        {
            try
            {
                var lista = await categoriasService.GetAll();
                Categorias = new ObservableCollection<CategoriaDTO>(lista);
                OnPropertyChanged(nameof(CategoriaSeleccionada));
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }

        private async Task GuardarAsync()
        {
            Mensaje = "";
            Producto.Nombre = (Producto.Nombre ?? "").Trim();

            if (Producto.Nombre.Length == 0)
            {
                Mensaje = "Escriba el nombre del producto.";
                return;
            }

            if (!decimal.TryParse(Precio, NumberStyles.Number, CultureInfo.CurrentCulture, out var precio) || precio <= 0)
            {
                Mensaje = "Escriba un precio válido mayor a cero.";
                return;
            }

            if (Producto.CategoriaId == 0)
            {
                Mensaje = "Seleccione la categoría.";
                return;
            }

            var dto = new ProductoCreateDTO
            {
                Nombre = Producto.Nombre,
                Precio = precio,
                Disponible = Producto.Disponible,
                IdCategoria = Producto.CategoriaId
            };

            try
            {
                if (original == null)
                {
                    await productosService.Create(dto);
                }
                else
                {
                    await productosService.Update(original.Id, dto);
                }

                principal.MostrarProductos();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}
