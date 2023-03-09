using Microsoft.AspNetCore.Mvc;

namespace jh_banno_assignment.Controllers;

[ApiController]
[Route("[controller]")]
public class ForecastController : ControllerBase
{
    public ForecastController(INoaaService noaaService, ILogger<ForecastController> logger) {
        _noaaService = noaaService;
        _logger = logger;
    }

    private readonly INoaaService _noaaService;
    private readonly ILogger<ForecastController> _logger;

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ForecastPeriodSummary>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetForecast(double latitude, double longitude)
    {
        // santitize given coord params
        if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180) {
            return BadRequest();
        }

        var coords = new LatLongPoint(latitude, longitude);
        var key = coords.GetHashCode();

        // update forecast cache item if needed
        if (CachedForecastNeedsUpdate(coords)) {
            // get NOAA gridpoint
            var gridPoint = await _noaaService.GetGridPoint(coords);
            
            if (gridPoint.HasValue) {
                // get NOAA forecast for gridpoint
                var resp = await _noaaService.GetForecast(gridPoint.Value);
                if (resp != null) {
                    var forecast = Convert.ToForecastSummary(resp, coords);
                    _forecastCache[key] = forecast;
                }
            }
        }

        if (_forecastCache.ContainsKey(key)) {
            return Ok(_forecastCache[key]);
        }
        else {
            return NotFound();
        }
    }

    // NOTE: this is a crude caching mechanism for forecasts. a more ideal solution would abstract a injectable
    // service that wrapped, say, a redis instance and allowed for tunable TTL, etc.
    private const int DefaultForecastCacheSeconds = 30;
    private static Dictionary<int, ForecastSummary> _forecastCache = new Dictionary<int, ForecastSummary>();

    private bool CachedForecastNeedsUpdate(LatLongPoint latLongPoint) {
        var key = latLongPoint.GetHashCode();

        if (_forecastCache.ContainsKey(key)) {
            var forecast = _forecastCache[key];
            if (forecast.RetrievedAt.AddSeconds(DefaultForecastCacheSeconds) >= DateTime.Now) {
                return false;
            }
        }

        return true;
    }
}
