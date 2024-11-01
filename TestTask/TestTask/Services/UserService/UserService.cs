using TestTask.Models;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Options;
using TestTask.Models.Settings;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace TestTask.Services.UserService
{
    public class UserService : IUserService
    {
        public UserService(IOptions<AppSettings> options)
        {
            _client = new HttpClient();
            _userApi = options.Value.UserApi;
        }

        public async Task<bool> Login(Credentials? credentials)
        {
            if (credentials == null) return false;

            var result = await _client.GetAsync($"{_userApi.Url}{_userApi.Endpoints.Login}?username={credentials.Login}&password={credentials.Password}");

            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> Logout()
        {
            var result = await _client.GetAsync($"{_userApi.Url}{_userApi.Endpoints.Logout}");

            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<User?> GetUser(string username)
        {
            var result = await _client.GetAsync($"{_userApi.Url}{_userApi.Endpoints.Get}{username}");
            
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var json = await result.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(json);

                return user;
            }

            return null;
        }

        public async Task<bool> Register(User? user)
        {
            if (user == null) return false;

            var body = JsonContent.Create(user);

            var result = await _client.PostAsync($"{_userApi.Url}{_userApi.Endpoints.Register}", body);

            var isRegistred = result.StatusCode == HttpStatusCode.OK;

            return isRegistred;
        }

        private HttpClient _client;

        private UserApi _userApi;
    }
}
