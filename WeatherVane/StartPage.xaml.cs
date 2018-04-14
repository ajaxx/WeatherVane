using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WeatherVane.Model;
using WeatherVane.Services;
using WeatherVane.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WeatherVane
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPage : Page
    {
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public StartPageViewModel ViewModel { get; set; } 

        public StartPage()
        {
            this.InitializeComponent();
            this.InitializeViewModel();
            this.InitializeLocation();
        }

        /// <summary>
        /// Initializes the view model.
        /// </summary>
        public void InitializeViewModel()
        {
            this.ViewModel = new StartPageViewModel();
        }

        /// <summary>
        /// Initializes the location of the user if they have allowed us to access
        /// that information.  If we are not allowed, then we will simply rely
        /// on the user to enter the information.
        /// </summary>
        public async void InitializeLocation()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus) {
                case GeolocationAccessStatus.Allowed:
                    var geoLocator = new Geolocator { DesiredAccuracy = PositionAccuracy.Default };
                    var geoPosition = await geoLocator.GetGeopositionAsync();

                    var geoCodingServiceManager = new GeocodingServiceManager();
                    var geoCodingService = geoCodingServiceManager.Instance;
                    var geoCodingResult = await geoCodingService.ResolveLocationFromCoordinates(
                        geoPosition.Coordinate.Point.Position.Latitude, geoPosition.Coordinate.Point.Position.Longitude);
                    if (string.IsNullOrEmpty(ViewModel.SearchText)) {
                        ViewModel.Location = geoCodingResult;
                        ViewModel.SearchText = geoCodingResult.ZipCode;
                    }
                    break;
                case GeolocationAccessStatus.Denied:
                case GeolocationAccessStatus.Unspecified:
                    break;
            }
        }

        private void OnSearchBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private async void OnGetWeatherRequest(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Location != null) {
                var regionalViewModel = new RegionalWeatherViewModel();
                regionalViewModel.Location = ViewModel.Location;
                regionalViewModel.Conditions = await GetCurrentConditions();
                regionalViewModel.DailyForecast = await GetDailyForecast();
                this.Frame.Navigate(typeof(RegionalWeatherPage), regionalViewModel);
            }
        }

        private Task<WeatherConditions> GetCurrentConditions()
        {
            var weatherServiceManager = new WeatherServiceManager();
            var weatherService = weatherServiceManager.Instance;
            return weatherService.GetCurrentConditions(ViewModel.Location);
        }

        private async Task<IList<WeatherForecast>> GetDailyForecast()
        {
            var weatherServiceManager = new WeatherServiceManager();
            var weatherService = weatherServiceManager.Instance;
            var result = await weatherService.GetForecast(ViewModel.Location);
            return result.ToList();
        }
    }
}
