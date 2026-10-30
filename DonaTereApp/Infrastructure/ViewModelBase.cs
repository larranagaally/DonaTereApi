using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PROYECTOFINAL_API.Infrastructure
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string nombre = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }

        protected bool Asignar<T>(ref T campo, T valor, [CallerMemberName] string nombre = "")
        {
            if (EqualityComparer<T>.Default.Equals(campo, valor)) return false;
            campo = valor;
            OnPropertyChanged(nombre);
            return true;
        }
    }
}
