using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace WeatherVane.Rest
{
    public class RestClient : IRestClient
    {
        private HttpClient _httpClient;

        /// <summary>
        /// Gets or sets the base address.
        /// </summary>
        /// <value>
        /// The base address.
        /// </value>
        public Uri BaseAddress { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class.
        /// </summary>
        public RestClient() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        public RestClient(Uri baseAddress)
        {
            BaseAddress = baseAddress;
        }

        /// <summary>
        /// Creates a request.
        /// </summary>
        /// <param name="resourcePath">The resourcePath.</param>
        /// <returns></returns>
        public IRestRequest CreateRequest(string resourcePath)
        {
            return new RestRequest(this, resourcePath);
        }

        /// <summary>
        /// Gets the HTTP client.  If one does not exist, this method creates one.
        /// </summary>
        /// <returns></returns>
        internal HttpClient GetHttpClient()
        {
            if (_httpClient == null) {
                var client = new HttpClient();
                client.BaseAddress = BaseAddress;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient = client;
            }

            return _httpClient;
        }

        public class RestRequest : IRestRequest
        {
            private readonly RestClient _restClient;
            private readonly string _resourcePath;
            private readonly ICollection<KeyValuePair<string, string>> _properties;

            /// <summary>
            /// Initializes a new instance of the <see cref="RestRequest"/> class.
            /// </summary>
            /// <param name="restClient">The rest client.</param>
            /// <param name="resourcePath">The resource resourcePath.</param>
            public RestRequest(RestClient restClient, string resourcePath)
            {
                _restClient = restClient;
                _resourcePath = resourcePath;
                _properties = new LinkedList<KeyValuePair<string, string>>();
            }

            /// <summary>
            /// Adds the property.  This method is additive only, we do not remove values
            /// that may have already been set.  In other words, there may be multiple
            /// values for the same key.  We are not handling that.
            /// </summary>
            /// <param name="name">The name.</param>
            /// <param name="value">The value.</param>
            public void AddProperty(string name, string value)
            {
                _properties.Add(new KeyValuePair<string, string>(name, value));
            }

            /// <summary>
            /// Executes an asynchronous GET request.
            /// </summary>
            public async Task<JsonObject> ExecuteGetAsync()
            {
                var requestPath = _resourcePath;
                var requestArgs = GetQueryString();
                if (!string.IsNullOrWhiteSpace(requestArgs)) {
                    requestPath += "?" + requestArgs;
                }

                var client = _restClient.GetHttpClient();
                var response = await client.GetAsync(_restClient.BaseAddress + requestPath);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadAsStringAsync();
                if (!JsonObject.TryParse(responseData, out var responseJson)) {
                    // In this case we were unable to parse the response.  This usually
                    // means the return value was not a Json structure, but it could
                    // also be due to various other issues.
                }

                // the Json structure is intact and parsed
                return responseJson;
            }

            /// <summary>
            /// Returns the properties as a query string.  This is useful for GET requests where
            /// the properties must be concatenated to the Uri.
            /// </summary>
            /// <returns>
            ///     The query string.
            /// </returns>
            private string GetQueryString()
            {
                var propertyEnum = _properties
                    .Select(entry => WebUtility.UrlEncode(entry.Key) + "=" + WebUtility.UrlEncode(entry.Value));
                return string.Join("&", propertyEnum);
            }
        }
    }
}
