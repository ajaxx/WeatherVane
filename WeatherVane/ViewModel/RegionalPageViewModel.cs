using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using WeatherVane.Model;

namespace WeatherVane.ViewModel
{
    public class RegionalPageViewModel : INotifyPropertyChanged
    {
        private ILocation _location;
        private WeatherConditions _conditions;
        private IList<WeatherForecast> _forecast;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        public ILocation Location
        {
            get { return _location; }
            set
            {
                _location = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the conditions.
        /// </summary>
        /// <value>
        /// The conditions.
        /// </value>
        public WeatherConditions Conditions {
            get { return _conditions; }
            set
            {
                _conditions = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the daily forecast.
        /// </summary>
        /// <value>
        /// The daily forecast.
        /// </value>
        public IList<WeatherForecast> DailyForecast {
            get { return _forecast; }
            set
            {
                _forecast = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionalPageViewModel"/> class.
        /// </summary>
        public RegionalPageViewModel()
        {
            _location = null;
            _conditions = null;
            _forecast = new List<WeatherForecast>();
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
