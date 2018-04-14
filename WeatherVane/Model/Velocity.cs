using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherVane.Model
{
    public class Velocity
    {
        private decimal _milesPerHour;

        /// <summary>
        /// Initializes a new instance of the <see cref="Velocity" /> class.
        /// </summary>
        /// <param name="milesPerHour">The miles per hour.</param>
        public Velocity(decimal milesPerHour)
        {
            _milesPerHour = milesPerHour;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Velocity"/> class.
        /// </summary>
        /// <param name="milesPerHour">The miles per hour.</param>
        public Velocity(double milesPerHour)
        {
            _milesPerHour = (decimal) milesPerHour;
        }

        public decimal Mph {
            get => _milesPerHour;
            set => _milesPerHour = value;
        }

        public decimal Kph {
            get => _milesPerHour * 1.60934m;
            set => _milesPerHour = value / 1.60934m;
        }
    }
}