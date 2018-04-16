using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Json;
using Windows.UI.ViewManagement;
using WeatherVane.Model;

namespace WeatherVane.Services
{
    /// <summary>
    /// A fake geocoding service.
    /// </summary>
    /// <seealso cref="WeatherVane.Services.IGeocodingService" />
    public class GeocodingServiceMock : IGeocodingService
    {
        private const double MaxDistance = 2000.0d;

        /// <summary>
        /// A fallback location
        /// </summary>
        private static ILocation DEFAULT_MOCK_LOCATION = new Address() {
            City = "Austin",
            State = "Texas",
            Country = "United States",
            ZipCode = "78758"
        };

        private IList<Place> _places;
        private ILocation _defaultLocation;

        /// <summary>
        /// Gets the places.  Use the search interface for queries.
        /// </summary>
        public IList<Place> Places => _places;

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
            _defaultLocation = DEFAULT_MOCK_LOCATION;
            InitializePlaces();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodingServiceMock"/> class.
        /// </summary>
        /// <param name="defaultLocation">The default location.</param>
        public GeocodingServiceMock(ILocation defaultLocation)
        {
            _defaultLocation = defaultLocation;
            InitializePlaces();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeocodingServiceMock"/> class.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="defaultLocation">The default location.</param>
        public GeocodingServiceMock(IList<GeocodingZone> points, ILocation defaultLocation)
        {
            _defaultLocation = defaultLocation;
            InitializePlaces();
        }

        /// <summary>
        /// Initializes the places.
        /// </summary>
        public void InitializePlaces()
        {
            var mockPath = Path.Combine(
                Package.Current.InstalledLocation.Path, "MockData", "MockGeocodingData.json");
            var mockJson = JsonArray.Parse(File.ReadAllText(mockPath));

            _places = mockJson
                .Select(element => element.GetObject())
                .Select(element => ElementToPlace(element))
                .ToList();
        }

        /// <summary>
        /// Converts a json element to a "place" object.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private static Place ElementToPlace(JsonObject element)
        {
            return new Place() {
                City = element.GetNamedString("City"),
                State = element.GetNamedString("State"),
                Zipcode = element.GetNamedString("Zipcode"),
                Longitude = element.GetNamedNumber("Long"),
                Latitude = element.GetNamedNumber("Lat")
            };
        }

        /// <summary>
        /// Searches for locations that match the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public Task<IList<ILocation>> Search(string query)
        {
            return Task.Run(
                () =>
                {
                    var searchTerm = query.ToUpperInvariant();
                    return (IList<ILocation>) _places
                        .Where(place => place.IsMatch(searchTerm))
                        .Select(place => place.Location)
                        .ToList();
                });
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
            foreach (var point in _places) {
                var distance = GetHaversineDistance(
                    latitude, point.Latitude,
                    longitude, point.Longitude);
                if (distance < MaxDistance) {
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

        public struct Place
        {
            public string City;
            public string State;
            public string Zipcode;
            public double Latitude;
            public double Longitude;

            public string DisplayName => $"{City}, {State} ({Zipcode})";

            public ILocation Location => new Address() {
                City = City,
                State = State,
                ZipCode = Zipcode
            };

            public bool IsMatch(string searchTerm)
            {
                return DisplayName.Contains(searchTerm);
            }
        }
    }
}
