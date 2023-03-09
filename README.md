## Jack Henry / Banno Assignment

Simple REST endpoint for retrieving forecast info from the NOAA API for the current day and given lat/long.

`ForecastController` defines a single endpoint, `/Forecast`, which takes `latitude` and `longitude` parameters on the query string.

It retrieves the `grid point` for the given coordinates from NOAA's `/points` endpoint, if one exists. If so, it then retrieves the current forecast for that grid point from the `/gridpoints` endpoint.

The default NOAA forecast data includes information for current day and following week. The forecast payload includes an array of `period` objects, which represent the forecast for a particular timespan, `startTime` and `endTime` in the period JSON.

For the current day, there can be `period` objects for 'today', 'this afternoon', and 'tonight'. Based on the time the NOAA forecast is retrieved, only the periods from the current time through the end of the day are included.

For the days following the current day, each day has a 'day' (6am to 6pm) and 'night' (6pm to 6am) period giving a rough forecast for each.

This endpoint only uses forecast data for the current day, see `Convert.ToForecastSummary`; everything else is discarded.

The raw NOAA forecast data is transformed into a summarized format that simply includes the short forecast string and a rough characterization of the temperature (is it `cold`, `moderate`, or `hot`) for each period of the current day's forecast.

This project uses .Net Core 6.0 and was tested on Ubuntu 20.04.

## Build

`./dotnet build`

## Run

`./dotnet run`

## Testing

In the interests of time, I omitted adding any automated tests around these components and the endpoint. It would be good to have confidence that invalid values for parameters to `ForecastController` and `NoaaService` methods will not cause the service to fail. The caching mechanism would benefit from having some tests around it to ensure that TTL is enforced correctly.

With the service running, `test.sh` will retrieve a few forecasts and store the response json in a `test-output` file.

## Improvements

Adding a circuit breaker, retry mechanism for the NoaaService would improve resilience in cases where we get rate limited or the NOAA API is unavailable.

If the caching mechanism was backed by a shared store (e.g. redis), scaling to multiple nodes would allow all the nodes to update and benefit from that cache.

The NOAA grid points probably do not change often. Moving those to a durable, shared store could eliminate one HTTP request to NOAA for each `/Forecast` invocation for each grid point that is in that store.

## Endpoints

`http --verify=no GET https://localhost:7232/Forecast?latitude={lat}&longitude={long}`

## Coords for a few cities
Chicago: 41.881832,-87.623177

Dallas:  32.779167,-96.808891

Denver: 39.742043,-104.991531

San Francisco: 37.773972,-122.431297

Seattle: 47.6079999,-122.335

Toronto: 43.70011000,-79.41630000

### NOAA API examples

get gridpoint from Seattle lat/long

`http https://api.weather.gov/points/47.608,-122.335`

get forecast for Seattle

`http GET https://api.weather.gov/gridpoints/SEW/124,67/forecast`