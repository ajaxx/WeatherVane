using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.UI.ViewManagement;
using WeatherVane.Model;
using WeatherVane.Services;

namespace WeatherVane.ViewModel
{
    public class StartPageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The search text
        /// </summary>
        private string _searchText;

        /// <summary>
        /// The currently selected location.
        /// </summary>
        private ILocation _location;

        /// <summary>
        /// Gets the weather service.
        /// </summary>
        /// <value>
        /// The weather service.
        /// </value>
        public IWeatherService WeatherService { get; private set; }

        /// <summary>
        /// Gets the geocoding service.
        /// </summary>
        /// <value>
        /// The geocoding service.
        /// </value>
        public IGeocodingService GeocodingService { get; private set; }

        /// <summary>
        /// Gets or sets the search text suggestions.
        /// </summary>
        /// <value>
        /// The search text suggestions.
        /// </value>
        public ObservableCollection<ILocation> SearchTextSuggestions { get; private set; }

        /// <summary>
        /// Gets the search history.  The search history contains all locations that
        /// have been selected by the user.
        /// </summary>
        public ObservableCollection<ILocation> LocationHistory { get; private set; }

        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>
        /// The search text.
        /// </value>
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public ILocation Location {
            get { return _location; }
            set
            {
                _location = value;
                OnPropertyChanged();

                if (value != null) {
                    if (LocationHistory.Count == 0) {
                        LocationHistory.Insert(0, value);
                    }
                    else {
                        int index = LocationHistory.IndexOf(value);
                        if (index == -1) {
                            LocationHistory.Insert(0, value);
                            while (LocationHistory.Count > 10) {
                                LocationHistory.RemoveAt(LocationHistory.Count - 1);
                            }
                        }
                        else if (index != 0) {
                            LocationHistory.Move(index, 0);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartPageViewModel"/> class.
        /// </summary>
        public StartPageViewModel()
        {
            InitializeGeocodingService();
            InitializeWeatherService();
            InitializeSuggestionCollection();
            InitializeLocationHistory();
            InitializeLocation();
        }

        /// <summary>
        /// Initializes the geocoding service.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void InitializeGeocodingService()
        {
            GeocodingService = (new GeocodingServiceManager()).Instance;
        }

        /// <summary>
        /// Initializes the suggestion collection.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void InitializeSuggestionCollection()
        {
            SearchTextSuggestions = new ObservableCollection<ILocation>();
        }

        /// <summary>
        /// Initializes the location history.
        /// </summary>
        private void InitializeLocationHistory()
        {
            var locationHistoryManager = new LocationHistoryManager();
            LocationHistory = locationHistoryManager.Instance.Locations;
        }

        /// <summary>
        /// Initializes the location of the user if they have allowed us to access
        /// that information.  If we are not allowed, then we will simply rely
        /// on the user to enter the information.
        /// </summary>
        public async void InitializeLocation()
        {
            if (!string.IsNullOrEmpty(SearchText)) {
                return;
            }

            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    var geoLocator = new Geolocator { DesiredAccuracy = PositionAccuracy.Default };
                    var geoPosition = await geoLocator.GetGeopositionAsync();

                    var geoCodingResult = await GeocodingService.ResolveLocationFromCoordinates(
                        geoPosition.Coordinate.Point.Position.Latitude, geoPosition.Coordinate.Point.Position.Longitude);

                    if (string.IsNullOrEmpty(SearchText))
                    {
                        Location = geoCodingResult;
                        SearchText = geoCodingResult.DisplayName;
                    }

                    break;
                case GeolocationAccessStatus.Denied:
                case GeolocationAccessStatus.Unspecified:
                    break;
            }
        }

        /// <summary>
        /// Initializes the weather service.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void InitializeWeatherService()
        {
            WeatherService = (new WeatherServiceManager()).Instance;
        }

        /// <summary>
        /// Updates the search suggestions.
        /// </summary>
        public async void UpdateSearchSuggestions()
        {
            UpdateSearchSuggestions(await GeocodingService.Search(SearchText));
        }

        /// <summary>
        /// Updates the search suggestions with a specific set of locations.
        /// </summary>
        /// <param name="locations">The locations.</param>
        public void UpdateSearchSuggestions(IList<ILocation> locations)
        {
            SearchTextSuggestions.Clear();
            foreach (var searchResult in locations.Take(10))
            {
                SearchTextSuggestions.Add(searchResult);
            }
        }

        /// <summary>
        /// Gets the current conditions.
        /// </summary>
        /// <returns></returns>
        private Task<WeatherConditions> GetCurrentConditions()
        {
            return WeatherService.GetCurrentConditions(Location);
        }

        /// <summary>
        /// Gets the daily forecast.
        /// </summary>
        /// <returns></returns>
        private async Task<IList<WeatherForecast>> GetDailyForecast()
        {
            var result = await WeatherService.GetForecast(Location);
            return result.ToList();
        }

        public async Task<RegionalPageViewModel> GetRegionalViewModelAsync()
        {
            if (Location != null)
            {
                var regionalViewModel = new RegionalPageViewModel();
                regionalViewModel.Location = Location;
                regionalViewModel.Conditions = await GetCurrentConditions();
                regionalViewModel.DailyForecast = await GetDailyForecast();
                return regionalViewModel;
            }

            return null;
        }

        /// <summary>
        /// Gets the regional view model for the current location.
        /// </summary>
        public RegionalPageViewModel GetRegionalViewModel()
        {
            return Task.Run(GetRegionalViewModelAsync).Result;
        }

        /// <summary>
        /// Called when a property changes.  Sends a notification to listeners that
        /// said property has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
