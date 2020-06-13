using System;
using System.Collections.Generic;
using System.Linq;

namespace Xalendar.Api.Models
{
    public class Month
    {
        public DateTime MonthDateTime {get;}
        public IReadOnlyList<Day> Days {get;}

        public Month(DateTime dateTime)
        {
            MonthDateTime = dateTime;
            Days = GenerateDaysOfMonth();
        }

        private List<Day> GenerateDaysOfMonth()
        {
            return Enumerable
                .Range(1, DateTime.DaysInMonth(MonthDateTime.Year, MonthDateTime.Month))
                .Select(dayValue => new DateTime(MonthDateTime.Year, MonthDateTime.Month, dayValue))
                .Select(dateTime => new Day(dateTime))
                .ToList();
        }
    }
}
