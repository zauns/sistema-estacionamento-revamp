using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkingSystem.Desktop.Models;
using ParkingSystem.Desktop.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ParkingSystem.Desktop.ViewModels
{
    // ObservableObject implementa INotifyPropertyChanged para nós.
    public partial class MainViewModel : ObservableObject
    {
        private readonly IParkingApiService _apiService;

        // A anotação [ObservableProperty] cria a propriedade e notifica a UI sobre mudanças.
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

        // A anotação [RelayCommand] cria um ICommand que a UI pode chamar.
        [RelayCommand]
        private async Task LoadParkingSpotsAsync()
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            
            var spotsFromApi = await _apiService.GetParkingSpotsAsync();

            if (spotsFromApi.Any())
            {
                ParkingSpots.Clear();
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
    }
}
