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
using WeatherVane.Model;
using WeatherVane.ViewModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeatherVane
{
    public sealed partial class DayForecastControl : UserControl
    {
        /// <summary>
        /// Gets or sets the forecast.
        /// </summary>
        /// <value>
        /// The forecast.
        /// </value>
        public WeatherForecast Forecast { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DayForecastControl"/> class.
        /// </summary>
        public DayForecastControl()
        {
            this.InitializeComponent();
        }
    }
}
