using System;
using System.Collections.Generic;
using System.Linq;

namespace Xalendar.Api.Models
{
    public class Month
    {
        private readonly DateTime _dateTime;
        private readonly IReadOnlyList<Day> _days;

        public Month(DateTime dateTime)
        {
            _dateTime = dateTime;
            _days = GenerateDaysOfMonth();
        }

        private List<Day> GenerateDaysOfMonth()
        {
            return Enumerable
                .Range(1, DateTime.DaysInMonth(_dateTime.Year, _dateTime.Month))
                .Select(dayValue => new DateTime(_dateTime.Year, _dateTime.Month, dayValue))
                .Select(dateTime => new Day(dateTime))
                .ToList();
        }

        public IReadOnlyList<Day> GetDaysOfMonth()
        {
            return _days;
        }

        public void SelectDay(Day selectedDay)
        {
            if (GetSelectedDay() is Day currentDaySelected)
            {
                currentDaySelected.IsSelected = false;
            }

            var newSelectedDay = GetDaysOfMonth()
                .FirstOrDefault(day => day.DateTime.Equals(selectedDay.DateTime));

            if (newSelectedDay is Day localNewSelectedDay)
            {
                localNewSelectedDay.IsSelected = true;
            }
        }

        public Day GetSelectedDay()
        {
            return GetDaysOfMonth()
                .ToList()
                .FirstOrDefault(day => day.IsSelected);
        }
    }
}
