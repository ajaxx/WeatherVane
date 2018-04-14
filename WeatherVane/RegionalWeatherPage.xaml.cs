using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WeatherVane.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeatherVane
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegionalWeatherPage : Page
    {
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public RegionalWeatherViewModel ViewModel { get; set; }

        public RegionalWeatherPage()
        {
            this.InitializeComponent();
            this.InitializeViewModel();
        }

        /// <summary>
        /// Initializes the view model.
        /// </summary>
        public void InitializeViewModel()
        {
            this.ViewModel = new RegionalWeatherViewModel();
        }

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.ViewModel = (RegionalWeatherViewModel) e.Parameter;
        }
    }
}
