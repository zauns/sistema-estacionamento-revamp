using ParkingSystem.Desktop.ViewModels;
using System.Windows;

namespace ParkingSystem.Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                // Chama o comando para carregar os dados iniciais
                await viewModel.LoadParkingSpotsCommand.ExecuteAsync(null);
            }
        }
    }
}