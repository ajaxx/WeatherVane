using System.Threading.Tasks;
using WeatherVane.Model;

namespace WeatherVane.Services
{
    /// <summary>
    /// Represents services that provide geocoding capabilities.
    /// </summary>
    public interface IGeocodingService
    {
        /// <summary>
        /// Resolves a location from coordinates.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns></returns>
        Task<ILocation> ResolveLocationFromCoordinates(double latitude, double longitude);
    }
}