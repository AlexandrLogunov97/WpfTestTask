using TestTask.Base;
using TestTask.Models;
using TestTask.Services.UserService;
using TestTask.States;

namespace TestTask.ViewModels.Auth
{
    public class NonAuthViewModel
    {
        public NonAuthViewModel(IUserService userService, State<AuthorizationState> state)
        {
            _userService = userService;

            State = state.Subscribe(AuthorizationState.Logging, Logging);

            Func<string, string, ValidationResult> IsNullOrWhiteSpaceRule = (value, message) =>
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return new ValidationResult(message);
                }

                return null;
            };
            LoginError = string.Empty;

            Login = new Property<string>(string.Empty)
                .SetValidationRule(x => IsNullOrWhiteSpaceRule(x, "Login is empty"))
                .SetHandler(x => LoginError.Value = string.Empty);
            Password = new Property<string>(string.Empty)
                .SetValidationRule(x => IsNullOrWhiteSpaceRule(x, "Password is empty"))
                .SetHandler(x => LoginError.Value = string.Empty);
        }

        public Command LoginCommand => new Command(
            x =>
            {
                if (Login.IsValid && Password.IsValid)
                {
                    State.Update(AuthorizationState.Logging, new Credentials(Login.Value, Password.Value));
                }
            },
            x => CanLogin());

        public Command StartRegisterCommand => new Command(
            x => State.Value = AuthorizationState.Registering,
            x => CanStartRegistering());

        private bool CanLogin()
        {
            return State.Value != AuthorizationState.Logging;
        }

        private bool CanStartRegistering()
        {
            return State.Value != AuthorizationState.Logging &&
                   State.Value != AuthorizationState.Logouting;
        }

        private async void Logging(object? parameter)
        {
            if (parameter is Credentials credentials)
            {
                if (credentials.Login != Login.Value && credentials.Password != Password.Value)
                {
                    Login.Value = credentials.Login;
                    Password.Value = credentials.Password;
                }

                var isLoggedIn = await _userService.Login(credentials);

                if (isLoggedIn)
                {
                    var user = await _userService.GetUser(credentials.Login);

                    if (user != null && credentials.Password == user.Password)
                    {
                        State.Update(AuthorizationState.LoggedIn, user);

                        Login.Value = string.Empty;
                        Password.Value = string.Empty;

                        return;
                    }
                }
            }

            LoginError.Value = "Something went wrong!";

            State.Value = AuthorizationState.LoggedOut;
        }

        public State<AuthorizationState> State { get; set; }

        public Property<string> LoginError { get; set; }

        public Property<string> Login { get; set; }

        public Property<string> Password { get; set; }

        private IUserService _userService;
    }
}
