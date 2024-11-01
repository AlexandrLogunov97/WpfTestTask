using TestTask.Base;

namespace TestTask.Models
{
    public class UserModel
    {
        public UserModel()
        {
            Username = new Property<string>(string.Empty)
                .SetValidationRule(x => IsNullOrWhiteSpace(x, "User name is empty"));

            FirstName = new Property<string>(string.Empty)
                .SetValidationRule(x => IsNullOrWhiteSpace(x, "First name is empty"));

            LastName = new Property<string>(string.Empty)
                .SetValidationRule(x => IsNullOrWhiteSpace(x, "Last name is empty"));

            Email = new Property<string>(string.Empty)
                .SetValidationRule(x => IsNullOrWhiteSpace(x, "Email name is empty"));

            Password = new Property<string>(string.Empty)
                .SetValidationRule(x => IsNullOrWhiteSpace(x, "Password name is empty"));

            Phone = new Property<string>(string.Empty)
                .SetValidationRule(x => IsNullOrWhiteSpace(x, "Phone name is empty"));
        }

        public static implicit operator UserModel(User value)
        {
            var userModel = new UserModel();
            userModel.Username.Value = value.Username;
            userModel.FirstName.Value = value.FirstName;
            userModel.LastName.Value = value.LastName;
            userModel.Email.Value = value.Email;
            userModel.Password.Value = value.Password;
            userModel.Phone.Value = value.Phone;

            return userModel;
        }

        public static implicit operator User?(UserModel userModel)
        {
            return new User
            {
                Username = userModel.Username,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                Password = userModel.Password,
                Phone = userModel.Phone
            };
        }

        private ValidationResult IsNullOrWhiteSpace(string? value, string? message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new ValidationResult(message);
            }

            return null;
        }

        public Property<string> Username { get; set; } = string.Empty;

        public Property<string> FirstName { get; set; } = string.Empty;

        public Property<string> LastName { get; set; } = string.Empty;

        public Property<string> Email { get; set; } = string.Empty;

        public Property<string> Password { get; set; } = string.Empty;

        public Property<string> Phone { get; set; } = string.Empty;
    }
}
