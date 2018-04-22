using System;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using WeatherVane.Model;
using WeatherVane.ViewModel;

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
            this.ViewModel.PropertyChanged += UpdateWeatherCommand;
            this.DataContext = ViewModel;
        }

        /// <summary>
        /// Updates the weather command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void UpdateWeatherCommand(object sender, PropertyChangedEventArgs e)
        {
            GetWeatherCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Called when [search box query submitted].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxQuerySubmittedEventArgs"/> instance containing the event data.</param>
        private async void OnSearchBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var location = (ILocation) args.ChosenSuggestion;
            if (location == null) {
                // The user could just hit enter - if this happens, we will get
                // no location, but valid query text.  Attempt to lookup the location
                // from the query text.
                var result = await ViewModel.GeocodingService.Search(args.QueryText);
                if (result.Count == 0) {
                    ViewModel.Location = null;
                    return;
                } else if (result.Count == 1) {
                    // There was one and only one result to this search.  In that case,
                    // we can use the result as the location.
                    location = result[0];
                }
                else {
                    // We have multiple locations.  In this case, we don't want to "select"
                    // a location, but rather this information needs to be used to populate
                    // the suggestion box.
                    ViewModel.Location = null;
                    ViewModel.UpdateSearchSuggestions(result);
                    return;
                }
            }

            if (location != null) {
                // the user chose an item from the autosuggest box - this is the
                // simplest use case to complete.
                this.ViewModel.Location = location;
                this.ViewModel.SearchText = location.DisplayName;
            }
        }

        /// <summary>
        /// Occurs when the text is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="AutoSuggestBoxTextChangedEventArgs"/> instance containing the event data.</param>
        private void OnSearchTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            switch (args.Reason) {
                case AutoSuggestionBoxTextChangeReason.UserInput:
                    ViewModel.UpdateSearchSuggestions();
                    break;
                case AutoSuggestionBoxTextChangeReason.ProgrammaticChange:
                case AutoSuggestionBoxTextChangeReason.SuggestionChosen:
                    break;
            }
        }

        /// <summary>
        /// Occurs when a historic location is selected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnHistoricLocationSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0) {
                var location = (ILocation) e.AddedItems[0];
                ViewModel.Location = location;
                ViewModel.SearchText = location.DisplayName;
            }
        }
    }
}
