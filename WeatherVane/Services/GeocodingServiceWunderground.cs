using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherVane.Model;
using WeatherVane.Rest;

namespace WeatherVane.Services
{
    public class GeocodingServiceWunderground : IGeocodingService
    {
        private static readonly Uri GeolocationUri = new Uri("http://api.wunderground.com/api");

        private static string ApiKey => "TBD";

        /// <summary>
        /// Resolves a location from coordinates.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ILocation> ResolveLocationFromCoordinates(double latitude, double longitude)
        {
            var point = string.Format("{0:0.####},{1:0.####}", latitude, longitude);

            var restClient = new RestClient(GeolocationUri);
            var request = restClient.CreateRequest(string.Format("/{0}/geolookup/q/{1}.json", ApiKey, point));

            var result = await request.ExecuteGetAsync();
            var location = result.GetNamedObject("location");
            var country = location.GetNamedString("country");
            var state = location.GetNamedString("state");
            var city = location.GetNamedString("city");
            var zip = location.GetNamedString("zip");

            return new Address() {
                City = city,
                State = state,
                Country = country,
                ZipCode = zip
            };
        }
    }
}
