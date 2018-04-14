using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherVane.Services
{
    /// <summary>
    /// Manages instances of the weather service that are handed out.  Future
    /// efforts might want to leverage an inversion of control or dependency
    /// injection model, but that is beyond the scope for this class.
    /// </summary>
    public class WeatherServiceManager
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public IWeatherService Instance { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherVane.Services.WeatherServiceManager"/> class.
        /// </summary>
        public WeatherServiceManager()
        {
            Instance = new WeatherServiceMock();
        }
    }
}
