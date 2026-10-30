using PROYECTOFINAL_API.Infrastructure;
using PROYECTOFINAL_API.Models;

namespace PROYECTOFINAL_API.ViewModels
{
    public class LineaPedido : ViewModelBase
    {
        private readonly Action alCambiar;
        private int cantidad;

        public LineaPedido(Productos producto, Action alCambiar)
        {
            Producto = producto;
            this.alCambiar = alCambiar;

            MasCommand = new RelayCommand(() => Cantidad = Cantidad + 1);
            MenosCommand = new RelayCommand(() =>
            {
                if (Cantidad > 0) Cantidad = Cantidad - 1;
            });
        }

        public Productos Producto { get; }

        public int Cantidad
        {
            get => cantidad;
            set
            {
                if (Asignar(ref cantidad, value))
                {
                    OnPropertyChanged(nameof(Subtotal));
                    alCambiar();
                }
            }
        }

        public decimal Subtotal => Cantidad * Producto.Precio;

        public RelayCommand MasCommand { get; }
        public RelayCommand MenosCommand { get; }
    }
}
