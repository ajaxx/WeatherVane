using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WeatherVane.Model;
using WeatherVane.Rest;

namespace WeatherVane.Services
{
    public class GeocodingServiceBing : IGeocodingService
    {
        private static readonly Uri GeolocationUri = new Uri("http://dev.virtualearth.net/REST/v1");

        private static string ApiKey => "TBD";

        public async Task<List<string>> QueryLocations(string query)
        {
            var restClient = new RestClient(GeolocationUri);
            var request = restClient.CreateRequest("/Locations");
            request.AddProperty("query", query);
            request.AddProperty("o", "json");
            request.AddProperty("key", ApiKey);

            var result = await request.ExecuteGetAsync();
            var suggestions = new List<string>();

            var resourceSetZero = result["resourceSets"]
                .GetArray()
                .GetObjectAt(0);
            var resourceZero = resourceSetZero
                .GetNamedArray("resources")
                .GetObjectAt(0);
            var resourceAddress = resourceZero
                .GetNamedObject("address");

            return suggestions;
        }

        /// <summary>
        /// Resolves the location from coordinates.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns></returns>
        public async Task<ILocation> ResolveLocationFromCoordinates(double latitude, double longitude)
        {
            var point = string.Format("{0:0.####},{1:0.####}", latitude, longitude);

            var restClient = new RestClient(GeolocationUri);
            var request = restClient.CreateRequest("/Locations/" + point);
            request.AddProperty("includeEntityTypes", "Postcode1");
            request.AddProperty("includeNeighborhood", "0");
            request.AddProperty("o", "json");
            request.AddProperty("key", ApiKey);

            var result = await request.ExecuteGetAsync();
            var resourceSetZero = result["resourceSets"]
                .GetArray()
                .GetObjectAt(0);
            var resourceZero = resourceSetZero
                .GetNamedArray("resources")
                .GetObjectAt(0);
            var resourceAddress = resourceZero
                .GetNamedObject("address");

            return new Address() {
                City = resourceAddress.GetNamedString("locality"),
                State = resourceAddress.GetNamedString("adminDistrict"),
                Country = resourceAddress.GetNamedString("countryRegion"),
                ZipCode = resourceAddress.GetNamedString("postalCode")
            };
        }
    }
}
