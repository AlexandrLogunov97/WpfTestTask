using Microsoft.Extensions.Options;
using TestTask.Base;
using TestTask.Models;
using TestTask.Models.Settings;
using TestTask.Services.UserService;
using TestTask.States;

namespace TestTask.ViewModels.Main
{
    public class MainViewModel
    {
        public MainViewModel(IUserService userService, IOptions<AppSettings> options)
        {
            State = new State<AuthorizationState>(AuthorizationState.LoggedOut)
                .Subscribe(AuthorizationState.LoggedIn, Authorized)
                .Subscribe(AuthorizationState.LoggedOut, x => Unauthorized());

            Main = new State<MainState>(MainState.View1);

            CurrentUser = new Property<User>(null);

            AuthViewModel = new AuthViewModel(userService, State);

            AppName = options.Value?.AppName;

            Developer = options.Value?.Developer;
        }


        public Command GoToViewCommand => new Command(x =>
        {
            if (x is MainState state)
            {
                Main.Value = state;
            }
        });

        private void Authorized(object? parameter)
        {
            if (parameter is User user)
            {
                CurrentUser.Value = user;
            }
        }

        private void Unauthorized()
        {
            CurrentUser.Value = null;
        }

        public string? AppName { get; set; }

        public DeveloperInfo? Developer { get; set; }

        public State<AuthorizationState> State { get; set; }

        public Property<User> CurrentUser { get; set; }

        public State<MainState> Main { get; set; }

        public AuthViewModel AuthViewModel { get; set; }
    }
}
