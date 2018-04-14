using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WeatherVane.Model;

namespace WeatherVane.Services
{
    /// <summary>
    /// A fake geocoding service.
    /// </summary>
    /// <seealso cref="WeatherVane.Services.IGeocodingService" />
    public class GeocodingServiceMock : IGeocodingService
    {
        private static ILocation DEFAULT_MOCK_LOCATION = new Address() {
            City = "Austin",
            State = "Texas",
            Country = "United States",
            ZipCode = "78758"
        };

        private IList<GeocodingZone> _points;
        private ILocation _defaultLocation;

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public IList<GeocodingZone> Points {
            get => _points;
            set => _points = value;
        }

        /// <summary>
        /// Gets or sets the default location.
        /// </summary>
        /// <value>
        /// The default location.
        /// </value>
        public ILocation DefaultLocation {
            get => _defaultLocation;
            set => _defaultLocation = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodingServiceMock"/> class.
        /// </summary>
        public GeocodingServiceMock()
        {
            _points = new List<GeocodingZone>();
            _defaultLocation = DEFAULT_MOCK_LOCATION;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodingServiceMock"/> class.
        /// </summary>
        /// <param name="defaultLocation">The default location.</param>
        public GeocodingServiceMock(ILocation defaultLocation)
        {
            _points = new List<GeocodingZone>();
            _defaultLocation = defaultLocation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodingServiceMock"/> class.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="defaultLocation">The default location.</param>
        public GeocodingServiceMock(IList<GeocodingZone> points, ILocation defaultLocation)
        {
            _points = points;
            _defaultLocation = defaultLocation;
        }

        /// <summary>
        /// Resolves a location from coordinates.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ILocation> ResolveLocationFromCoordinates(double latitude, double longitude)
        {
            foreach (var point in _points) {
                var distance = GetHaversineDistance(
                    latitude, point.Latitude,
                    longitude, point.Longitude);
                if (distance < point.Distance) {
                    return point.Location;
                }
            }

            return _defaultLocation;
        }

        public static double GetHaversineDistance(
            double lat1, double lat2,
            double lon1, double lon2)
        {
            const double r = 6371e3;
            const double degreesToRadians = Math.PI / 180.0;
            double phi1 = degreesToRadians * lat1;
            double phi2 = degreesToRadians * lat2;
            double deltaPhi = (lat2 - lat1) * degreesToRadians;
            double deltaLambda = (lon2 - lon1) * degreesToRadians;
            double a = (
                Math.Sin(deltaPhi / 2.0) * Math.Sin(deltaPhi / 2.0) +
                Math.Cos(phi1) * Math.Cos(phi2) *
                Math.Sin(deltaLambda / 2.0) * Math.Sin(deltaLambda / 2.0));
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = r * c;

            return d;
        }

        public struct GeocodingZone
        {
            public double Latitude;
            public double Longitude;
            public double Distance;
            public ILocation Location;
        }
    }
}
