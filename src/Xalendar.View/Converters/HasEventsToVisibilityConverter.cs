using System;
using System.Globalization;
using Xalendar.Api.Models;
using Xamarin.Forms;

namespace Xalendar.View.Converters
{
    public class HasEventsToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Day day)
                return day.HasEvents;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
