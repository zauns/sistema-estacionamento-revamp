using ParkingSystem.Web.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ParkingSystem.Web.Services
{
    public class ParkingApiService : IParkingApiService
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "https://localhost:7079";

        public ParkingApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ParkingSpotDto>> GetParkingSpotsAsync()
        {
            try
            {
                var spots = await _httpClient.GetFromJsonAsync<List<ParkingSpotDto>>($"{ApiBaseUrl}/api/parkingspots");
                return spots ?? new List<ParkingSpotDto>();
            }
            catch (HttpRequestException ex)
            {
                // TODO: Logging de erro 
                System.Diagnostics.Debug.WriteLine($"API connection error: {ex.Message}");
                return new List<ParkingSpotDto>();
            }
        }

        public async Task<VehicleViewModel?> GetVehicleInSpotAsync(int spotId)
        {
            // Endpoint improvisado
            var response = await _httpClient.GetAsync($"{ApiBaseUrl}/api/vehicles/parked");
            if (!response.IsSuccessStatusCode) return null;

            var parkedVehicles = await response.Content.ReadFromJsonAsync<List<VehicleViewModel>>();
            return parkedVehicles?.FirstOrDefault(v => v.ParkingSpotId == spotId);
        }

        public async Task<VehicleViewModel?> ParkVehicleAsync(ParkVehicleRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/api/vehicles/park", request);
            if (!response.IsSuccessStatusCode)
            {
                // TODO: Logging de erro
                return null;
            }
            return await response.Content.ReadFromJsonAsync<VehicleViewModel>();
        }

        public async Task<ParkingTicketViewModel?> RemoveVehicleAsync(string licensePlate)
        {
            var response = await _httpClient.PostAsync($"{ApiBaseUrl}/api/vehicles/exit/{licensePlate}", null);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<ParkingTicketViewModel>();
        }
        public async Task<int[]?> GetTodayEntriesByHourAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<int[]>($"{ApiBaseUrl}/api/vehicles/history/today-by-hour");
        }
        catch (HttpRequestException ex)
        {
            System.Diagnostics.Debug.WriteLine($"API connection error for chart data: {ex.Message}");
            return null;
        }
    }
    }
}