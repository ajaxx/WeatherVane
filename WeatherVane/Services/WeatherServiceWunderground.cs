using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.Data.Json;

using WeatherVane.Model;
using WeatherVane.Rest;
using WeatherVane.Utility;

using static WeatherVane.Utility.ModelExtensions;

namespace WeatherVane.Services
{
    public class WeatherServiceWunderground : IWeatherService
    {
        private static readonly Uri GeolocationUri = new Uri("http://api.wunderground.com/api");

        private static string ApiKey => "TBD";

        private WeatherForecast CreateForecastFromJson(JsonObject jsonObject)
        {
            var dateEpochString = jsonObject
                .GetNamedObject("date")
                .GetNamedString("epoch");
            var dateEpoch = Int64.Parse(dateEpochString);

            return new WeatherForecast()
            {
                Date = DateTimeHelper.FromEpoch(dateEpoch),
                Low = ParseTemperature(jsonObject.GetNamedObject("low").GetNamedString("fahrenheit")),
                High = ParseTemperature(jsonObject.GetNamedObject("high").GetNamedString("fahrenheit")),
                Conditions = jsonObject.GetNamedString("conditions")
            };
        }

        /// <summary>
        /// Gets the forecast.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<WeatherForecast>> GetForecast(ILocation location)
        {
            var restClient = new RestClient(GeolocationUri);
            var request = restClient.CreateRequest(string.Format("/{0}/forecast10day/q/{1}.json", ApiKey, location.ZipCode));
            var response = await request.ExecuteGetAsync();
            var forecastResult = new List<WeatherForecast>();
            var forecast = response.GetNamedObject("forecast").GetNamedObject("simpleforecast");
            var forecastArray = forecast.GetNamedArray("forecastday");

            foreach (var dailyForecastData in forecastArray) {
                forecastResult.Add(
                    CreateForecastFromJson(dailyForecastData.GetObject()));
            }

            return forecastResult;
        }

        /// <summary>
        /// Gets the current conditions.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<WeatherConditions> GetCurrentConditions(ILocation location)
        {
            var restClient = new RestClient(GeolocationUri);
            var request = restClient.CreateRequest(string.Format("/{0}/conditions/q/{1}.json", ApiKey, location.ZipCode));
            var response = await request.ExecuteGetAsync();
            var observation = response.GetNamedObject("current_observation");

            return new WeatherConditions() {
                WindDirection = observation.GetNamedString("wind_dir"),
                WindDegrees = observation.GetNamedNumber("wind_degrees"),
                WindSpeed = new Velocity(observation.GetNamedNumber("wind_mph")),
                WindGusts = ParseVelocity(observation.GetNamedString("wind_gust_mph")),
                Temperature = new Temperature(observation.GetNamedNumber("temp_f")),
                Dewpoint = new Temperature(observation.GetNamedNumber("dewpoint_f")),
                //RelativeHumidity = observation.GetNamedString("relative_humidity"),
                WindChill = ParseTemperature(observation.GetNamedString("windchill_f")),
                FeelsLike = ParseTemperature(observation.GetNamedString("feelslike_f"))
            };
        }
    }
}
