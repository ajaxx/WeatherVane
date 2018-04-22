using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherVane.Model;

namespace WeatherVane.Services
{
    /// <summary>
    /// A non-persistent history of locations.
    /// </summary>
    public class LocationHistoryInMemory : ILocationHistory
    {
        /// <summary>
        /// Gets the collection of historic locations.
        /// </summary>
        public ObservableCollection<ILocation> Locations { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationHistoryInMemory"/> class.
        /// </summary>
        public LocationHistoryInMemory()
        {
            Locations = new ObservableCollection<ILocation>();
        }
    }
}
