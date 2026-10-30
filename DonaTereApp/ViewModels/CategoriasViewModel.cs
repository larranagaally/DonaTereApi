using System.Collections.ObjectModel;
using DoñaTere.Core.DTOs;
using PROYECTOFINAL_API.Infrastructure;
using DonaTereApp.Services;

namespace PROYECTOFINAL_API.ViewModels
{
    public class CategoriasViewModel : ViewModelBase
    {
        private readonly MainViewModel principal;
        private readonly CategoriasService categoriasService = new();
        private string mensaje = "";

        public CategoriasViewModel(MainViewModel principal)
        {
            this.principal = principal;
            Categorias = new ObservableCollection<CategoriaDTO>();

            NuevoCommand = new RelayCommand(principal.MostrarAgregarCategoria);
            EditarCommand = new RelayCommand(c => principal.MostrarEditarCategoria((CategoriaDTO)c!));
            EliminarCommand = new RelayCommand(c => _ = EliminarAsync((CategoriaDTO)c!));

            _ = CargarAsync();
        }

        public ObservableCollection<CategoriaDTO> Categorias { get; }

        public string Mensaje { get => mensaje; set => Asignar(ref mensaje, value); }

        public RelayCommand NuevoCommand { get; }
        public RelayCommand EditarCommand { get; }
        public RelayCommand EliminarCommand { get; }

        public async Task CargarAsync()
        {
            try
            {
                var lista = await categoriasService.GetAll();
                Categorias.Clear();
                foreach (var c in lista) Categorias.Add(c);
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }

        private async Task EliminarAsync(CategoriaDTO categoria)
        {
            try
            {
                await categoriasService.Delete(categoria.Id);
                Mensaje = "Categoría eliminada.";
                await CargarAsync();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}
