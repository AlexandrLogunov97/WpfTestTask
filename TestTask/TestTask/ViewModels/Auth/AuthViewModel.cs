using TestTask.Base;
using TestTask.Services.UserService;
using TestTask.States;
using TestTask.ViewModels.Auth;

namespace TestTask.ViewModels
{
    public class AuthViewModel
    {
        public AuthViewModel(IUserService userService, State<AuthorizationState> state)
        {
            _userService = userService;

            State = state;

            NonAuthViewModel = new NonAuthViewModel(userService, state);

            RegisterViewModel = new RegisterViewModel(userService, state);
        }

        public Command LogoutCommand => new Command(async x =>
        {
            State.Value = AuthorizationState.Logouting;

            var isLogouted = await _userService.Logout();

            if (isLogouted)
            {
                State.Value = AuthorizationState.LoggedOut;
            }
        }, x => CanLogout());

        private bool CanLogout()
        {
            return State.Value != AuthorizationState.LoggedOut &&
                   State.Value != AuthorizationState.Logouting;
        }

        public NonAuthViewModel NonAuthViewModel { get; set; }

        public RegisterViewModel RegisterViewModel { get; set; }

        public State<AuthorizationState> State { get; set; }

        private IUserService _userService;
    }
}
