using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Xalendar.Api.Extensions;

namespace Xalendar.Api.Models
{
    public class MonthContainer
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Month _month;

        private IReadOnlyList<Day?>? _days;
        public IReadOnlyList<Day?> Days => _days ??= GetDaysOfContainer();
        public IReadOnlyList<string> DaysOfWeek { get; }
        
        public MonthContainer(DateTime dateTime)
        {
            _month = new Month(dateTime);

            DaysOfWeek = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>()
                .Select(GetDayOfWeekAbbreviated)
                .ToList();
        }

        private string GetDayOfWeekAbbreviated(DayOfWeek dayOfWeek)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedDayName(dayOfWeek)
                .Substring(0, 3).ToUpper();
        }

        private IReadOnlyList<Day?> GetDaysOfContainer()
        {
            var daysOfContainer = new List<Day?>();
            this.GetDaysToDiscardAtStartOfMonth(daysOfContainer);
            daysOfContainer.AddRange(_month.Days);
            this.GetDaysToDiscardAtEndOfMonth(daysOfContainer);
            return daysOfContainer;
        }
        
        public void Next()
        {
            var nextDateTime = _month.MonthDateTime.AddMonths(1);
            _month = new Month(nextDateTime);
            _days = null;
        }

        public void Previous()
        {
            var previousDateTime = _month.MonthDateTime.AddMonths(-1);
            _month = new Month(previousDateTime);
            _days = null;
        }
    }
}
