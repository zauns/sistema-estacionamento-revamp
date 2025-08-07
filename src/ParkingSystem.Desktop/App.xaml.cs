using Microsoft.Extensions.DependencyInjection;
using ParkingSystem.Desktop.Services;
using ParkingSystem.Desktop.ViewModels;
using System;
using System.Windows;

namespace ParkingSystem.Desktop
{
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Registra o HttpClient para ser usado pelo nosso serviço de API
            services.AddHttpClient<IParkingApiService, ParkingApiService>();

            // Registra o ViewModel. Transient significa que um novo será criado cada vez que for solicitado.
            services.AddTransient<MainViewModel>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);

                if (ServiceProvider == null)
                {
                    MessageBox.Show("Erro interno: ServiceProvider não foi inicializado.",
                                    "Erro de Inicialização",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    this.Shutdown();
                    return;
                }

                var mainWindow = new MainWindow
                {
                    // Pega o MainViewModel do contêiner de DI e o define como DataContext
                    DataContext = ServiceProvider.GetRequiredService<MainViewModel>()
                };
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inicializar aplicação: {ex.Message}", 
                               "Erro de Inicialização", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Error);
                this.Shutdown();
            }
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Erro não tratado: {e.Exception.Message}", 
                           "Erro", 
                           MessageBoxButton.OK, 
                           MessageBoxImage.Error);
            e.Handled = true;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (ServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
            base.OnExit(e);
        }
    }
}