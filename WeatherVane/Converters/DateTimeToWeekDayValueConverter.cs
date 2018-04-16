using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace WeatherVane.Converters
{
    public class DateTimeToWeekDayValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var dateTimeValue = (DateTime) value;
            return dateTimeValue.ToString("ddd").ToUpperInvariant();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
