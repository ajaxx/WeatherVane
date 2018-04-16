using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using WeatherVane.Model;

namespace WeatherVane.Converters
{
    public class TemperatureToStringValueConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>
        /// The units.
        /// </value>
        public TemperatureUnits Units { get; set; } = TemperatureUnits.Fahrenheit;

        /// <summary>
        /// Gets or sets the decimal places.
        /// </summary>
        /// <value>
        /// The decimal places.
        /// </value>
        public int DecimalPlaces { get; set; } = 0;

        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var temperature = (Temperature) value;
            switch (Units) {
                case TemperatureUnits.Fahrenheit:
                    return Math.Round(temperature.Fahrenheit, DecimalPlaces).ToString();
                case TemperatureUnits.Celcius:
                    return Math.Round(temperature.Celcius, DecimalPlaces).ToString();
                default:
                    throw new ArgumentException("invalid units of measure");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
