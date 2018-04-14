using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherVane.Model
{
    /// <summary>
    /// Represents a temperature which can be measured in fahrenheit or celcius
    /// </summary>
    public class Temperature
    {
        private const decimal CelciusToFahrenheitFactor = 5.0m / 9.0m;

        private decimal _tempInFahrenheit;

        /// <summary>
        /// Initializes a new instance of the <see cref="Temperature"/> class.
        /// </summary>
        /// <param name="tempInFahrenheit">The temporary in fahrenheit.</param>
        public Temperature(decimal tempInFahrenheit)
        {
            _tempInFahrenheit = tempInFahrenheit;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Temperature"/> class.
        /// </summary>
        /// <param name="tempInFahrenheit">The temporary in fahrenheit.</param>
        public Temperature(double tempInFahrenheit)
        {
            _tempInFahrenheit = (decimal) tempInFahrenheit;
        }

        public decimal Fahrenheit {
            get => _tempInFahrenheit;
            set => _tempInFahrenheit = value;
        }

        public decimal Celcius {
            get => (_tempInFahrenheit - 32) * CelciusToFahrenheitFactor;
            set => _tempInFahrenheit = (value / CelciusToFahrenheitFactor) + 32.0m;
        }
    }
}
