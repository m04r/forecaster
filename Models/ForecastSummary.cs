namespace jh_banno_assignment;

public enum TemperatureCharacterizations {
    None,
    Cold,
    Moderate,
    Hot
}

public class ForecastSummary {
    public ForecastSummary() {
        Periods = new List<ForecastPeriodSummary>();
    }

    public LatLongPoint Coordinates { get; set; }
    public DateTime GeneratedAt { get; set; }
    public List<ForecastPeriodSummary> Periods { get; set; }

    // used to determine invalidation for cached forecasts
    public DateTime RetrievedAt { get; set; }
}

public class ForecastPeriodSummary
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public string? ShortForecast { get; set; }

    public string? Characterization { get; set; }

}
