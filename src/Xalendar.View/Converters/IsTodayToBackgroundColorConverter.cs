using System;
using System.Globalization;
using Xalendar.Api.Models;
using Xamarin.Forms;

namespace Xalendar.View.Converters
{
    public class IsTodayToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Day day)
                if (day != null)
                    return day.IsToday ? Color.Red : Color.Transparent;

            return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
