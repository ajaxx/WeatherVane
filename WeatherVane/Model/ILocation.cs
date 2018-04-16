using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherVane.Model
{
    public interface ILocation
    {
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        string City { get; }
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        string State { get; }
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        string Country { get; }
        /// <summary>
        /// Gets or sets the postal code value.
        /// </summary>
        string ZipCode { get; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        string DisplayName { get; }
    }
}
