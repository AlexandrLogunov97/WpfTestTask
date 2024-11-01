namespace TestTask.Base
{
    public class ValidationResult
    {
        public ValidationResult(string? errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string? ErrorMessage { get; private set; }
    }

    public class Property<T> : BaseNotifyPropertyChanged<T>
    {
        public Property(T? value) : base(value)
        {
            _rule = (x) => null;
            _handler = (x) => { };
        }

        public static implicit operator Property<T>(T value)
        {
            return new Property<T>(value);
        }

        public static implicit operator T?(Property<T> property)
        {
            return property.Value;
        }

        public Property<T> SetValidationRule(Func<T?, ValidationResult?> rule)
        {          
            _rule = rule;
            ValidationCheck();

            return this;
        }

        public Property<T> SetHandler(Action<T?> handler)
        {
            _handler = handler;
            return this;
        }

        protected override void OnValueChanged()
        {
            ValidationCheck();
            _handler(Value);
        }

        private void ValidationCheck()
        {
            var validationResult = _rule(Value);

            IsValid = validationResult == null;
            ErrorMessage = IsValid ? string.Empty : validationResult?.ErrorMessage;

            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(ErrorMessage));
        }

        public bool IsValid { get; private set; }

        public string? ErrorMessage { get; private set; }

        private Func<T?, ValidationResult?> _rule;

        private Action<T?> _handler;
    }
}
