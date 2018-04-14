using System.Threading.Tasks;

using Windows.Data.Json;

namespace WeatherVane.Rest
{
    public interface IRestRequest
    {
        /// <summary>
        /// Adds the property.  This method is additive only, we do not remove values
        /// that may have already been set.  In other words, there may be multiple
        /// values for the same key.  We are not handling that.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        void AddProperty(string name, string value);

        /// <summary>
        /// Executes an asynchronous GET request.
        /// </summary>
        Task<JsonObject> ExecuteGetAsync();
    }
}