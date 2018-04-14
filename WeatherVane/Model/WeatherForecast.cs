using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherVane.Model
{
    /// <summary>
    /// Represents the weather forecast.
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// Gets or sets the date of the forecast.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Gets or sets the low temperature.
        /// </summary>
        public Temperature Low { get; set; }
        /// <summary>
        /// Gets or sets the high temperature.
        /// </summary>
        public Temperature High { get; set; }

        /// <summary>
        /// Gets or sets the conditions.
        /// </summary>
        public string Conditions { get; set; }
    }
}
