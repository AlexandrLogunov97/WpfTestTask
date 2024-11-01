using TestTask.Base;
using TestTask.Models;
using TestTask.Services.UserService;
using TestTask.States;

namespace TestTask.ViewModels.Auth
{
    public class RegisterViewModel
    {
        public RegisterViewModel(IUserService userService, State<AuthorizationState> state)
        {
            _userService = userService;

            State = state;

            NewUser = new Property<UserModel>(new UserModel());

            IsInRegistering = new Property<bool>(false);
        }

        private void Clear()
        {
            
        }

        public Command CancelCommand => new Command(x =>
        {
            NewUser.Value = new UserModel();
            State.Value = AuthorizationState.LoggedOut;
        }, x => !IsInRegistering.Value);

        public Command RegisterCommand => new Command(async x =>
        {
            IsInRegistering.Value = true;

            var isRegistred = await _userService.Register(NewUser.Value);

            if (isRegistred)
            {
                var login = NewUser.Value?.Username;
                var password = NewUser.Value?.Password;
                var credentials = new Credentials(login, password);
                State.Update(AuthorizationState.Logging, credentials);

                NewUser.Value = new UserModel();
            }

            IsInRegistering.Value = false;
        }, x => !IsInRegistering.Value);

        public State<AuthorizationState> State { get; set; }

        public Property<UserModel> NewUser { get; set; }

        public Property<bool> IsInRegistering { get; set; }

        private IUserService _userService;
    }
}
