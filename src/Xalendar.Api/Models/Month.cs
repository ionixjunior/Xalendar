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
            Day? currentDaySelected = GetSelectedDay();

            if (currentDaySelected.HasValue)
            {
                var localCurrentDaySelected = currentDaySelected.Value;
                localCurrentDaySelected.IsSelected = false;
            }

            Day? newSelectedDay = GetDaysOfMonth()
                .FirstOrDefault(day => day.DateTime.Equals(selectedDay.DateTime));

            if (newSelectedDay.HasValue)
            {
                var localNewSelectedDay = newSelectedDay.Value;
                localNewSelectedDay.IsSelected = true;
            }
        }

        public Day? GetSelectedDay()
        {
            Day? selectdDay = GetDaysOfMonth()
                .ToList()
                .FirstOrDefault(day => day.IsSelected);
            
            return selectdDay.Equals(default(Day)) ? null : selectdDay;
        }
    }
}
