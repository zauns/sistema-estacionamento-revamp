using ParkingSystem.Web.Models;
using ParkingSystem.Web.Exceptions;
using System.Net;
using System.Text.Json;

namespace ParkingSystem.Web.Services
{
    public class ParkingApiService : IParkingApiService
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "https://localhost:7079";
        private const int MaxRetryAttempts = 3;
        private const int RetryDelayMs = 1000;

        public ParkingApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromSeconds(10); // 10 second timeout
        }

        public async Task<List<ParkingSpotDto>> GetParkingSpotsAsync()
        {
            return await ExecuteWithRetryAsync(async () =>
            {
                try
                {
                    var response = await _httpClient.GetAsync($"{ApiBaseUrl}/api/parkingspots");
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ApiDataException(
                            GetErrorMessageForStatusCode(response.StatusCode, "vagas de estacionamento"),
                            (int)response.StatusCode);
                    }

                    var spots = await response.Content.ReadFromJsonAsync<List<ParkingSpotDto>>();
                    
                    if (spots == null)
                    {
                        throw new ApiDataException("Resposta da API está vazia ou inválida", 200);
                    }

                    return spots;
                }
                catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
                {
                    throw new ApiTimeoutException("Timeout ao carregar vagas de estacionamento", ex);
                }
                catch (HttpRequestException ex)
                {
                    throw new ApiConnectionException("Erro de conexão ao carregar vagas de estacionamento", ex);
                }
                catch (JsonException ex)
                {
                    throw new ApiDataException("Erro ao processar dados das vagas de estacionamento", 200);
                }
            });
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
    }
}