using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Json;

using WeatherVane.Model;
using WeatherVane.Utility;

using static WeatherVane.Utility.ModelExtensions;

namespace WeatherVane.Services
{
    public class WeatherServiceMock : IWeatherService
    {
        /// <summary>
        /// Gets the forecast.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<IEnumerable<WeatherForecast>> GetForecast(ILocation location)
        {
            return Task.Run(
                () =>
                {
                    var mockForecastPath = Path.Combine(
                        Package.Current.InstalledLocation.Path, "MockData", "MockWeatherForecast.json");
                    var mockForecastContent = File.ReadAllText(mockForecastPath);
                    var mockForecastJson = JsonObject.Parse(mockForecastContent);
                    var mockForecastData = mockForecastJson
                        .GetNamedObject("forecast")
                        .GetNamedObject("simpleforecast")
                        .GetNamedArray("forecastday");

                    return (IEnumerable<WeatherForecast>) mockForecastData
                        .Select(element => element.GetObject())
                        .Select(element => ElementToWeatherForecast(element))
                        .ToList();
                });
        }

        /// <summary>
        /// Converts a json object to a weather forecast.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private static WeatherForecast ElementToWeatherForecast(JsonObject element)
        {
            var dateEpoch = Int64.Parse(element.GetNamedObject("date").GetNamedString("epoch"));

            return new WeatherForecast() {
                Date = DateTimeHelper.FromEpoch(dateEpoch),
                Low = ParseTemperature(element.GetNamedObject("low").GetNamedString("fahrenheit")),
                High = ParseTemperature(element.GetNamedObject("high").GetNamedString("fahrenheit")),
                Conditions = element.GetNamedString("conditions")
            };
        }

        /// <summary>
        /// Gets the current conditions.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<WeatherConditions> GetCurrentConditions(ILocation location)
        {
            return Task.Run(
                () =>
                {
                    var mockForecastPath = Path.Combine(
                        Package.Current.InstalledLocation.Path, "MockData", "MockWeatherConditions.json");
                    var mockForecastContent = File.ReadAllText(mockForecastPath);
                    var mockForecastJson = JsonObject.Parse(mockForecastContent);
                    var mockForecastData = mockForecastJson
                        .GetNamedObject("current_observation");

                    return new WeatherConditions() {
                        Temperature = new Temperature(
                            mockForecastData.GetNamedNumber("temp_f")),
                        Dewpoint = new Temperature(
                            mockForecastData.GetNamedNumber("dewpoint_f")),
                        FeelsLike = ParseTemperature(
                            mockForecastData.GetNamedString("feelslike_f")),
                        HeatIndex = ParseTemperature(
                            mockForecastData.GetNamedString("heat_index_f")),
                        WindChill = ParseTemperature(
                            mockForecastData.GetNamedString("windchill_f")),
                        WindDirection = mockForecastData.GetNamedString("wind_dir"),
                        WindGusts = ParseVelocity(
                            mockForecastData.GetNamedString("wind_gust_mph")),
                        WindSpeed = new Velocity(
                            mockForecastData.GetNamedNumber("wind_mph")),
                        RelativeHumidity = mockForecastData.GetNamedString("relative_humidity")
                    };
                });
        }
    }
}
