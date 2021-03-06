﻿using System;
using System.Globalization;
using Xalendar.Api.Interfaces;

namespace Xalendar.Api.Formatters
{
    public class DayOfWeek1CaractersFormat : IDayOfWeekFormatter
    {
        public string Format(DayOfWeek dayOfWeek)
        {
            return CultureInfo
                .CurrentCulture
                .DateTimeFormat
                .GetDayName(dayOfWeek)
                .Substring(0, 1)
                .ToLower();
        }
    }
}
