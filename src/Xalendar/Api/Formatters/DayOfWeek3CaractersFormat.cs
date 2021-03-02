using System;
using System.Globalization;
using Xalendar.Api.Interfaces;

namespace Xalendar.Api.Formatters
{
    public class DayOfWeek3CaractersFormat : IDayOfWeekFormatter
    {
        public string Format(DayOfWeek dayOfWeek)
        {
            return CultureInfo
                .CurrentCulture
                .DateTimeFormat
                .GetDayName(dayOfWeek)
                .Substring(0, 3)
                .ToLower();
        }
    }
}
