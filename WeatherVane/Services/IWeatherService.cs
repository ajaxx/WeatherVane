using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeatherVane.Model;

namespace WeatherVane.Services
{
    public interface IWeatherService
    {
        /// <summary>
        /// Gets the forecast.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        Task<IEnumerable<WeatherForecast>> GetForecast(ILocation location);

        /// <summary>
        /// Gets the current conditions.
        /// </summary>
        /// <returns></returns>
        Task<WeatherConditions> GetCurrentConditions(ILocation location);
    }
}
