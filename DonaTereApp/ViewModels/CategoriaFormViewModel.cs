using DoñaTere.Core.DTOs;
using PROYECTOFINAL_API.Infrastructure;
using DonaTereApp.Services;

namespace PROYECTOFINAL_API.ViewModels
{
    public class CategoriaFormViewModel : ViewModelBase
    {
        private readonly MainViewModel principal;
        private readonly CategoriaDTO? original;
        private readonly CategoriasService categoriasService = new();
        private string mensaje = "";

        public CategoriaFormViewModel(MainViewModel principal, CategoriaDTO? original)
        {
            this.principal = principal;
            this.original = original;

            Categoria = new CategoriaCreateDTO { Nombre = original?.Nombre ?? "" };

            GuardarCommand = new RelayCommand(() => _ = GuardarAsync());
            VerCategoriasCommand = new RelayCommand(principal.MostrarCategorias);
        }

        public string Titulo => original == null ? "Nueva categoría" : "Editar categoría";

        public CategoriaCreateDTO Categoria { get; }

        public string Mensaje { get => mensaje; set => Asignar(ref mensaje, value); }

        public RelayCommand GuardarCommand { get; }
        public RelayCommand VerCategoriasCommand { get; }

        private async Task GuardarAsync()
        {
            Mensaje = "";
            Categoria.Nombre = (Categoria.Nombre ?? "").Trim();

            if (Categoria.Nombre.Length == 0)
            {
                Mensaje = "Escriba el nombre de la categoría.";
                return;
            }

            try
            {
                if (original == null)
                {
                    await categoriasService.Create(Categoria);
                }
                else
                {
                    await categoriasService.Update(original.Id, Categoria);
                }

                principal.MostrarCategorias();
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
            }
        }
    }
}
