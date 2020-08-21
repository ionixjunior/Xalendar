using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Xalendar.Api.Extensions;

namespace Xalendar.Api.Models
{
    public class MonthContainer
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Month _month;

        public IReadOnlyList<Day?> Days => GetDaysOfContainer();
        public IReadOnlyList<string> DaysOfWeek { get; }
        
        public MonthContainer(DateTime dateTime)
        {
            _month = new Month(dateTime);

            var cultureInfo = CultureInfo.CurrentCulture;
            var dateTimeFormat = cultureInfo.DateTimeFormat;
            DaysOfWeek = new List<string>
            {
                dateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Sunday).Substring(0, 3).ToUpper(),
                dateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Monday).Substring(0, 3).ToUpper(),
                dateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Tuesday).Substring(0, 3).ToUpper(),
                dateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Wednesday).Substring(0, 3).ToUpper(),
                dateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Thursday).Substring(0, 3).ToUpper(),
                dateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Friday).Substring(0, 3).ToUpper(),
                dateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Saturday).Substring(0, 3).ToUpper()
            };
        }

        private IReadOnlyList<Day?> GetDaysOfContainer()
        {
            var daysOfContainer = new List<Day?>();
            this.GetDaysToDiscardAtStartOfMonth(daysOfContainer);
            daysOfContainer.AddRange(_month.Days);
            this.GetDaysToDiscardAtEndOfMonth(daysOfContainer);
            return daysOfContainer;
        }
    }
}
