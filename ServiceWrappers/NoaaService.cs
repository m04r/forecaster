using System.Net.Http.Headers;
using System.Text.Json;

namespace jh_banno_assignment;

public interface INoaaService {
    Task<NoaaGridPoint?> GetGridPoint(LatLongPoint coords);
    Task<NoaaForecast?> GetForecast(NoaaGridPoint gridPoint);
}

public class NoaaService : INoaaService {
    public NoaaService(ILogger<NoaaService> logger) {
        _gridPointClient = GetHttpClient("https://api.weather.gov");
        _forecastClient = GetHttpClient("https://api.weather.gov");
        _logger = logger;
    }
    
    private readonly ILogger<NoaaService> _logger;
    private readonly HttpClient _gridPointClient;
    private readonly HttpClient _forecastClient;

    private HttpClient GetHttpClient(string baseUri) {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://api.weather.gov");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("User-Agent", "http client");

        return client;
    }

    public async Task<NoaaGridPoint?> GetGridPoint(LatLongPoint coords) {
        NoaaGridPoint? gridPoint = null;
        const string gridPointEndpointFormat = "points/{0},{1}";
        var endpoint = String.Format(gridPointEndpointFormat, coords.Lat, coords.Long);

        try {
            NoaaRawGridPointObject? rawGridPoint = await _forecastClient.GetFromJsonAsync<NoaaRawGridPointObject>(endpoint);
            if (rawGridPoint != null) {
                gridPoint = JsonSerializer.Deserialize<NoaaGridPoint>(rawGridPoint.Properties);
            }
        } catch (HttpRequestException hre) {
            _logger.LogWarning(hre, "Failed to retrieve grid point for lat/long {0}", coords);
        }

        return gridPoint;
    }

    public async Task<NoaaForecast?> GetForecast(NoaaGridPoint gridPoint)
    {
        NoaaForecast? forecast = null;
        const string forecastEndpointFormat = "gridpoints/{0}/{1},{2}/forecast";
        var endpoint = String.Format(forecastEndpointFormat, gridPoint.Id, gridPoint.X, gridPoint.Y);
        try {
            NoaaRawForecastObject? rawForecast = await _forecastClient.GetFromJsonAsync<NoaaRawForecastObject>(endpoint);
            if (rawForecast != null) {
                forecast = JsonSerializer.Deserialize<NoaaForecast>(rawForecast.Properties);
            }
        } catch (HttpRequestException hre) {
            _logger.LogWarning(hre, "Failed to retrieve forecast for gridpoint {0}", gridPoint);
        }

        return forecast;
    }
}