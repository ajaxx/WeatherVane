using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
        /// Gets the get weather command.
        /// </summary>
        /// <value>
        /// The get weather command.
        /// </value>
        public ProxyCommand GetWeatherCommand { get; private set; }
        
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public StartPageViewModel ViewModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartPage"/> class.
        /// </summary>
        public StartPage()
        {
            this.InitializeComponent();
            this.InitializeCommands();
            this.InitializeViewModel();
        }

        /// <summary>
        /// Initializes the commands.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void InitializeCommands()
        {
            GetWeatherCommand = new ProxyCommand(
                param => ViewModel.Location != null,
                param => Frame.Navigate(typeof(RegionalPage), ViewModel.GetRegionalViewModel()));
        }

        /// <summary>
        /// Initializes the view model.
        /// </summary>
        public void InitializeViewModel()
        {
            this.ViewModel = new StartPageViewModel();
            this.ViewModel.PropertyChanged += (sender, args) => GetWeatherCommand.RaiseCanExecuteChanged();
            this.DataContext = ViewModel;
        }

        /// <summary>
        /// Called when [search box query submitted].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxQuerySubmittedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="NotImplementedException"></exception>
        private async void OnSearchBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var location = (ILocation) args.ChosenSuggestion;
            if (location == null) {
                // The user could just hit enter - if this happens, we will get
                // no location, but valid query text.  Attempt to lookup the location
                // from the query text.
                var result = await ViewModel.GeocodingService.Search(args.QueryText);
                if (result.Count == 0) {
                    return;
                }

                // We're left with multiple results all of which could match.  We
                // will choose the first item that matches.
                location = result[0];
            }

            if (location != null) {
                // the user chose an item from the autosuggest box - this is the
                // simplest use case to complete.
                this.ViewModel.Location = location;
                // still not sure why the view is showing a version of the object
                // other than the display name...
                this.ViewModel.SuspendUpdateSearch = true;
                this.ViewModel.SearchText = location.DisplayName;
                this.ViewModel.SuspendUpdateSearch = false;
            }
        }
    }
}
