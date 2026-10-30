using System.Windows.Input;

namespace PROYECTOFINAL_API.Infrastructure
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> ejecutar;
        private readonly Func<bool>? puedeEjecutar;

        public RelayCommand(Action ejecutar, Func<bool>? puedeEjecutar = null)
        {
            this.ejecutar = _ => ejecutar();
            this.puedeEjecutar = puedeEjecutar;
        }

        public RelayCommand(Action<object?> ejecutar)
        {
            this.ejecutar = ejecutar;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => puedeEjecutar?.Invoke() ?? true;

        public void Execute(object? parameter) => ejecutar(parameter);
    }
}
