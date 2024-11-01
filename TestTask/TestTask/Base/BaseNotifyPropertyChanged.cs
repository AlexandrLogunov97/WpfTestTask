using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TestTask.Base
{
    public abstract class BaseNotifyPropertyChanged<T> : INotifyPropertyChanged
    {
        protected BaseNotifyPropertyChanged(T? value)
        {
            _value = value;
        }

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        protected virtual void OnValueChanged() { }

        public event PropertyChangedEventHandler? PropertyChanged;

        public T? Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
                OnValueChanged();
            }
        }

        private T? _value;
    }
}
