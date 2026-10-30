using DoñaTere.Core.DTOs;
using Microsoft.Maui;
using Microsoft.Maui.Devices;
using PROYECTOFINAL_API.Infrastructure;
using PROYECTOFINAL_API.Models;

namespace PROYECTOFINAL_API.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string pantalla = "";

        public MainViewModel()
        {
            InicioCommand = new RelayCommand(MostrarInicio);
            ProductosCommand = new RelayCommand(MostrarProductos);
            PedidosCommand = new RelayCommand(MostrarPedidos);
            ClientesCommand = new RelayCommand(MostrarClientes);
            CategoriasCommand = new RelayCommand(MostrarCategorias);
            ReportesCommand = new RelayCommand(MostrarReportes);

            MostrarInicio();
        }

        public RelayCommand InicioCommand { get; }
        public RelayCommand ProductosCommand { get; }
        public RelayCommand PedidosCommand { get; }
        public RelayCommand ClientesCommand { get; }
        public RelayCommand CategoriasCommand { get; }
        public RelayCommand ReportesCommand { get; }

        public bool EsCelular => DeviceInfo.Idiom == DeviceIdiom.Phone;
        public bool EsEscritorio => !EsCelular;

        public Thickness PaddingContenido => EsCelular ? new Thickness(12) : new Thickness(20);

        public InicioViewModel? InicioVM { get; private set; }
        public ProductosViewModel? ProductosVM { get; private set; }
        public PedidosViewModel? PedidosVM { get; private set; }
        public ClientesViewModel? ClientesVM { get; private set; }
        public CategoriasViewModel? CategoriasVM { get; private set; }
        public ReportesViewModel? ReportesVM { get; private set; }

        public AgregarPedidoViewModel? PedidoFormVM { get; private set; }
        public AgregarClienteViewModel? ClienteFormVM { get; private set; }
        public AgregarProductosViewModel? ProductoFormVM { get; private set; }
        public CategoriaFormViewModel? CategoriaFormVM { get; private set; }

        public bool EsInicio => pantalla == "Inicio";
        public bool EsProductos => pantalla == "Productos";
        public bool EsPedidos => pantalla == "Pedidos";
        public bool EsClientes => pantalla == "Clientes";
        public bool EsCategorias => pantalla == "Categorias";
        public bool EsReportes => pantalla == "Reportes";
        public bool EsPedidoForm => pantalla == "PedidoForm";
        public bool EsClienteForm => pantalla == "ClienteForm";
        public bool EsProductoForm => pantalla == "ProductoForm";
        public bool EsCategoriaForm => pantalla == "CategoriaForm";

        public void MostrarInicio()
        {
            InicioVM = new InicioViewModel();
            Ir("Inicio");
        }

        public void MostrarProductos()
        {
            ProductosVM = new ProductosViewModel(this);
            Ir("Productos");
        }

        public void MostrarPedidos()
        {
            PedidosVM = new PedidosViewModel(this);
            Ir("Pedidos");
        }

        public void MostrarClientes()
        {
            ClientesVM = new ClientesViewModel(this);
            Ir("Clientes");
        }

        public void MostrarCategorias()
        {
            CategoriasVM = new CategoriasViewModel(this);
            Ir("Categorias");
        }

        public void MostrarReportes()
        {
            ReportesVM = new ReportesViewModel();
            Ir("Reportes");
        }

        public void MostrarAgregarPedido()
        {
            PedidoFormVM = new AgregarPedidoViewModel(this);
            Ir("PedidoForm");
        }

        public void MostrarAgregarCliente()
        {
            ClienteFormVM = new AgregarClienteViewModel(this, null);
            Ir("ClienteForm");
        }

        public void MostrarEditarCliente(ClienteDTO cliente)
        {
            ClienteFormVM = new AgregarClienteViewModel(this, cliente);
            Ir("ClienteForm");
        }

        public void MostrarAgregarProducto()
        {
            ProductoFormVM = new AgregarProductosViewModel(this, null);
            Ir("ProductoForm");
        }

        public void MostrarEditarProducto(Productos producto)
        {
            ProductoFormVM = new AgregarProductosViewModel(this, producto);
            Ir("ProductoForm");
        }

        public void MostrarAgregarCategoria()
        {
            CategoriaFormVM = new CategoriaFormViewModel(this, null);
            Ir("CategoriaForm");
        }

        public void MostrarEditarCategoria(CategoriaDTO categoria)
        {
            CategoriaFormVM = new CategoriaFormViewModel(this, categoria);
            Ir("CategoriaForm");
        }

        private void Ir(string destino)
        {
            pantalla = destino;

            OnPropertyChanged(nameof(EsInicio));
            OnPropertyChanged(nameof(EsProductos));
            OnPropertyChanged(nameof(EsPedidos));
            OnPropertyChanged(nameof(EsClientes));
            OnPropertyChanged(nameof(EsCategorias));
            OnPropertyChanged(nameof(EsReportes));
            OnPropertyChanged(nameof(EsPedidoForm));
            OnPropertyChanged(nameof(EsClienteForm));
            OnPropertyChanged(nameof(EsProductoForm));
            OnPropertyChanged(nameof(EsCategoriaForm));

            OnPropertyChanged(nameof(InicioVM));
            OnPropertyChanged(nameof(ProductosVM));
            OnPropertyChanged(nameof(PedidosVM));
            OnPropertyChanged(nameof(ClientesVM));
            OnPropertyChanged(nameof(CategoriasVM));
            OnPropertyChanged(nameof(ReportesVM));
            OnPropertyChanged(nameof(PedidoFormVM));
            OnPropertyChanged(nameof(ClienteFormVM));
            OnPropertyChanged(nameof(ProductoFormVM));
            OnPropertyChanged(nameof(CategoriaFormVM));
        }
    }
}
