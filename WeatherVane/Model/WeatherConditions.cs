using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherVane.Model
{
    public class WeatherConditions
    {
        /// <summary>
        /// Gets or sets the wind direction.
        /// </summary>
        /// <value>
        /// The wind direction.
        /// </value>
        public string WindDirection { get; set; }
        /// <summary>
        /// Gets or sets the wind degrees.
        /// </summary>
        /// <value>
        /// The wind degrees.
        /// </value>
        public double WindDegrees { get; set; }
        /// <summary>
        /// Gets or sets the wind speed.
        /// </summary>
        /// <value>
        /// The wind speed.
        /// </value>
        public Velocity WindSpeed { get; set; }
        /// <summary>
        /// Gets or sets the wind gusts.
        /// </summary>
        /// <value>
        /// The wind gusts.
        /// </value>
        public Velocity WindGusts { get; set; }

        /// <summary>
        /// Gets or sets the relative humidity.
        /// </summary>
        /// <value>
        /// The relative humidity.
        /// </value>
        public string RelativeHumidity { get; set; }
        /// <summary>
        /// Gets or sets the temperature.
        /// </summary>
        /// <value>
        /// The temperature.
        /// </value>
        public Temperature Temperature { get; set; }
        /// <summary>
        /// Gets or sets the wind chill.
        /// </summary>
        /// <value>
        /// The wind chill.
        /// </value>
        public Temperature WindChill { get; set; }
        /// <summary>
        /// Gets or sets the feels like.
        /// </summary>
        /// <value>
        /// The feels like.
        /// </value>
        public Temperature FeelsLike { get; set; }
        /// <summary>
        /// Gets or sets the index of the heat.
        /// </summary>
        /// <value>
        /// The index of the heat.
        /// </value>
        public Temperature HeatIndex { get; set; }
        /// <summary>
        /// Gets or sets the dewpoint.
        /// </summary>
        /// <value>
        /// The dewpoint.
        /// </value>
        public Temperature Dewpoint { get; set; }
        /// <summary>
        /// Gets or sets the conditions.
        /// </summary>
        public string Conditions { get; set; }
        /// <summary>
        /// Gets or sets the conditions description.
        /// </summary>
        public string ConditionsDescription { get; set; }
        /// <summary>
        /// Gets or sets the conditions icon URI.
        /// </summary>
        public string ConditionsIconUri { get; set; }
    }
}
