using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Text;
using System.Windows;
using TestTask.Services.UserService;
using TestTask.Models.Settings;
using TestTask.ViewModels.Main;

namespace TestTask
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();

            ConfigureService(services);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = new MainWindow();

            mainWindow.DataContext = ServiceProvider.GetRequiredService<MainViewModel>();

            mainWindow.Show();
        }

        private void ConfigureService(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddTransient<IUserService, UserService>();

            services.AddTransient<MainViewModel>();
        }

        public IServiceProvider? ServiceProvider { get; set; }

        public IConfiguration? Configuration { get; set; }
    }
}
