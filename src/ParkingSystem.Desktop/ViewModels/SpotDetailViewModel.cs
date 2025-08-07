// ViewModels/SpotDetailViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ParkingSystem.Desktop.Models;
using ParkingSystem.Desktop.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using ParkingSystem.Core.DTOs;

namespace ParkingSystem.Desktop.ViewModels
{
    public partial class SpotDetailViewModel : ObservableObject
    {
        private readonly IParkingApiService _apiService;
        private readonly Action _closeWindow;

        [ObservableProperty]
        private ParkingSpotDto _spot;

        [ObservableProperty]
        private string _newVehicleLicensePlate = string.Empty;
        
        [ObservableProperty]
        private string _newVehicleModel = string.Empty;
        
        [ObservableProperty]
        private string _newVehicleColor = string.Empty;

        [ObservableProperty]
        private VehicleViewModel? _vehicleDetails;

        [ObservableProperty]
        private bool _isBusy = false;

        [ObservableProperty]
        private string? _actionMessage;

        public SpotDetailViewModel(ParkingSpotDto spot, IParkingApiService apiService, Action closeWindow)
        {
            _spot = spot;
            _apiService = apiService;
            _closeWindow = closeWindow;

            // Se a vaga estiver ocupada, carrega os detalhes do veículo
            if (_spot.IsOccupied)
            {
                LoadVehicleDetailsCommand.Execute(null);
            }
        }

        [RelayCommand]
        private async Task LoadVehicleDetailsAsync()
        {
            IsBusy = true;
            VehicleDetails = await _apiService.GetVehicleInSpotAsync(Spot.Id);
            IsBusy = false;
        }

        [RelayCommand]
        private async Task ParkVehicleAsync()
        {
            if (string.IsNullOrWhiteSpace(NewVehicleLicensePlate))
            {
                MessageBox.Show("A placa do veículo é obrigatória.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IsBusy = true;
            var request = new ParkVehicleRequest
            {
                SpotId = Spot.Id,
                LicensePlate = NewVehicleLicensePlate,
                Model = NewVehicleModel,
                Color = NewVehicleColor
            };

            var result = await _apiService.ParkVehicleAsync(request);
            IsBusy = false;

            if (result != null)
            {
                MessageBox.Show($"Veículo {result.LicensePlate} estacionado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                _closeWindow();
            }
            else
            {
                MessageBox.Show("Não foi possível estacionar o veículo. Verifique os dados e se a vaga ainda está livre.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task RemoveVehicleAsync()
        {
            if (VehicleDetails == null) return;

            var confirmation = MessageBox.Show($"Deseja realmente liberar a vaga e remover o veículo {VehicleDetails.LicensePlate}?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirmation == MessageBoxResult.No) return;

            IsBusy = true;
            var result = await _apiService.RemoveVehicleAsync(VehicleDetails.LicensePlate);
            IsBusy = false;

            if (result != null)
            {
                MessageBox.Show($"Veículo removido.\n\nTempo Estacionado: {result.TimeParked}\nValor a Pagar: {result.TotalAmount:C}", "Ticket de Saída", MessageBoxButton.OK, MessageBoxImage.Information);
                _closeWindow();
            }
            else
            {
                MessageBox.Show("Não foi possível remover o veículo.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
