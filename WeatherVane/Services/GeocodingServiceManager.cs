using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherVane.Services
{
    /// <summary>
    /// Manages instances of the geocoding service that are handed out.  Future
    /// efforts might want to leverage an inversion of control or dependency
    /// injection model, but that is beyond the scope for this class.
    /// </summary>
    public class GeocodingServiceManager
    {
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public IGeocodingService Instance { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodingServiceManager"/> class.
        /// </summary>
        public GeocodingServiceManager()
        {
            Instance = new GeocodingServiceMock();
        }
    }
}
