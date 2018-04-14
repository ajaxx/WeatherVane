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
            }
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
