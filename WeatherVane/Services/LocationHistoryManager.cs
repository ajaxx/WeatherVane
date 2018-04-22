namespace WeatherVane.Services
{
    /// <summary>
    /// Manages instances of the location history that are handed out.  Future
    /// efforts might want to leverage an inversion of control or dependency
    /// injection model, but that is beyond the scope for this class.
    /// </summary>
    public class LocationHistoryManager
    {
        private static readonly ILocationHistory ApplicationInstance = new LocationHistoryInMemory();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public ILocationHistory Instance => ApplicationInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationHistoryManager"/> class.
        /// </summary>
        public LocationHistoryManager()
        {
        }
    }
}
