using ParkingSystem.Desktop.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json; 
using System.Threading.Tasks;

namespace ParkingSystem.Desktop.Services
{
    public class ParkingApiService : IParkingApiService
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "http://localhost:5163"; 

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
    }
}