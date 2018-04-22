using System.Collections.ObjectModel;
using WeatherVane.Model;

namespace WeatherVane.Services
{
    /// <summary>
    /// Exposes behavior that tracks location over time.
    /// </summary>
    public interface ILocationHistory
    {
        /// <summary>
        /// Gets the collection of historic locations.
        /// </summary>
        ObservableCollection<ILocation> Locations { get; }
    }
}
