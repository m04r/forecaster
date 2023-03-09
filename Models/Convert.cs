namespace jh_banno_assignment;

public static class Convert {
    public static ForecastSummary ToForecastSummary(NoaaForecast noaaForecast, LatLongPoint coords) {
        return new ForecastSummary() {
            GeneratedAt = noaaForecast.GeneratedAt,
            RetrievedAt = DateTime.Now,
            Coordinates = coords,
            // filter out any periods in the forecast that are not for today
            Periods = noaaForecast.Periods
                .FindAll(p => p.StartTime.DayOfYear == DateTime.Now.DayOfYear)
                .Select(p => ToForecastPeriodSummary(p)).ToList()
        };
    }

    public static ForecastPeriodSummary ToForecastPeriodSummary(NoaaForecastPeriod noaaForecastPeriod) {
        var characterization = MapTemperatureCharacterization(noaaForecastPeriod);

        return new ForecastPeriodSummary() {
            Start = noaaForecastPeriod.StartTime,
            End = noaaForecastPeriod.EndTime,
            Characterization = characterization,
            ShortForecast = noaaForecastPeriod.ShortForecast
        };
    }

    // NOTE: these ranges could be made more flexible, pulled from config settings or something.
    public static string MapTemperatureCharacterization(NoaaForecastPeriod noaaForecastPeriod) {
        if (noaaForecastPeriod.Temperature < 40) {
            return TemperatureCharacterizations.Cold.ToString();
        }
        else if (noaaForecastPeriod.Temperature >= 40 && noaaForecastPeriod.Temperature < 85) {
            return TemperatureCharacterizations.Moderate.ToString();
        }
        else if (noaaForecastPeriod.Temperature >= 85) {
            return TemperatureCharacterizations.Hot.ToString();
        }

        return TemperatureCharacterizations.None.ToString();
    }    
}