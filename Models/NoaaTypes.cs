namespace jh_banno_assignment;

using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

internal class NoaaRawGridPointObject {
    [JsonPropertyName("properties")]
    public JsonNode? Properties { get; set; }

}

public struct NoaaGridPoint {
    public static readonly NoaaGridPoint None = new NoaaGridPoint(string.Empty, int.MinValue, int.MinValue);

    public NoaaGridPoint(string gridId, int x, int y) {
        Id = gridId;
        X = x;
        Y = y;
    }

    [JsonPropertyName("gridId")]
    public string Id { get; set; }
    [JsonPropertyName("gridX")]
    public int X { get; set; }
    [JsonPropertyName("gridY")]
    public int Y { get; set; }
}

internal class NoaaRawForecastObject {
    [JsonPropertyName("properties")]
    public JsonNode? Properties { get; set; }
}

public class NoaaForecast {
    public NoaaForecast() {
        Periods = new List<NoaaForecastPeriod>();
    }

    public NoaaGridPoint GridPoint { get; set; }
    
    [JsonPropertyName("generatedAt")]
    public DateTime GeneratedAt { get; set; }

    [JsonPropertyName("periods")]
    public List<NoaaForecastPeriod> Periods { get; set; }
}

public class NoaaForecastPeriod {
    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; set; }
    [JsonPropertyName("endTime")]
    public DateTime EndTime { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("shortForecast")]
    public string? ShortForecast { get; set; }

    [JsonPropertyName("temperature")]
    public int Temperature { get; set; }
}
