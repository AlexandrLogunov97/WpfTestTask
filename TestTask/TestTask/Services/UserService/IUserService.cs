using TestTask.Models;

namespace TestTask.Services.UserService
{
    public interface IUserService
    {
        Task<bool> Login(Credentials? credentials);

        Task<bool> Logout();

        Task<User?> GetUser(string? username);

        Task<bool> Register(User? user);
    }
}
