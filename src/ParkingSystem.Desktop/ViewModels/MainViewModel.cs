using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkingSystem.Desktop.Models;
using ParkingSystem.Desktop.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ParkingSystem.Desktop.Views;

namespace ParkingSystem.Desktop.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        
        private readonly IParkingApiService _apiService;

        [ObservableProperty]
        private ObservableCollection<ParkingSpotDto> _parkingSpots = new();

        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        public MainViewModel(IParkingApiService apiService)
        {
            _apiService = apiService;
        }

        [RelayCommand]
        private async Task LoadParkingSpotsAsync()
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            ParkingSpots.Clear(); // Limpa para dar feedback visual imediato
            
            var spotsFromApi = await _apiService.GetParkingSpotsAsync();

            if (spotsFromApi.Any())
            {
                foreach (var spot in spotsFromApi)
                {
                    ParkingSpots.Add(spot);
                }
            }
            else
            {
                ErrorMessage = "Não foi possível carregar as vagas. Verifique se a API está em execução.";
            }
            
            IsLoading = false;
        }

        [RelayCommand]
        private void ShowSpotDetails(ParkingSpotDto? spot)
        {
            if (spot == null) return;

            SpotDetailWindow? detailWindow = null;

            // Criamos uma função para ser passada ao ViewModel, permitindo que ele feche a janela.
            Action closeAction = () => detailWindow?.Close();
            
            // Passamos a instância do serviço e a ação de fechar para o ViewModel.
            var detailViewModel = new SpotDetailViewModel(spot, _apiService, closeAction);

            detailWindow = new SpotDetailWindow
            {
                DataContext = detailViewModel,
                Owner = Application.Current.MainWindow
            };

            detailWindow.ShowDialog();

            // Após o diálogo fechar, atualiza a lista de vagas.
            LoadParkingSpotsCommand.Execute(null);
        }
    }
}
