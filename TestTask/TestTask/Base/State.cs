namespace TestTask.Base
{
    public class State<T> : BaseNotifyPropertyChanged<T>
        where T : notnull, Enum
    {
        public State(T value) : base(value)
        {
            _triggers = new Dictionary<T, Action<object?>>();
        }

        public static implicit operator State<T>(T value)
        {
            return new State<T>(value);
        }

        public static implicit operator T?(State<T> state)
        {
            return state.Value;
        }

        public State<T> Subscribe(T trigger, Action<object?> action)
        {
            if (trigger != null)
            {
                _triggers.TryAdd(trigger, action);
            }

            return this;
        }

        public void Update(T value, object? parameter)
        {
            if (value != null && _triggers.TryGetValue(value, out var trigger))
            {
                trigger.Invoke(parameter);
            }

            Value = value;
        }

        private Dictionary<T, Action<object?>> _triggers;
    }
}
