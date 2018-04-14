using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherVane.Model
{
    public class Address : ILocation
    {
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Gets or sets the postal code value.
        /// </summary>
        public string ZipCode { get; set; }
    }
}
