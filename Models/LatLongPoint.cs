namespace jh_banno_assignment;

public struct LatLongPoint {
    public LatLongPoint(double latitude, double longitude) {
        Lat = latitude;
        Long = longitude;
    }

    // the NOAA API only accepts lat/long values 3 or fewer decimal places
    private double _lat = double.MinValue;
    public double Lat { 
        get {
            return Math.Round(_lat, 3);
        }
        set {
            _lat = value;
        }
    }
    private double _long = double.MinValue;
    public double Long { 
        get {
            return Math.Round(_long, 3);
        }
        set {
            _long = value;
        }
    }

    // this is used as the key for a caching layer for forecasts
    public override int GetHashCode()
    {
        return HashCode.Combine(Lat, Long);
    }

    public override string ToString()
    {
        return "{" + Lat + "," + Long + "}";
    }
}

