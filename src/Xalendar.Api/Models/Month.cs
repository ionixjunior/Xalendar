using System;
using System.Collections.Generic;
using System.Linq;

namespace Xalendar.Api.Models
{
    public struct Month
    {
        private readonly DateTime _dateTime;
        private readonly IReadOnlyList<Day> _days;

        public Month(DateTime dateTime) : this()
        {
            _dateTime = dateTime;
            _days = GenerateDaysOfMonth();
        }

        private List<Day> GenerateDaysOfMonth()
        {
            DateTime instanceDateTime = _dateTime;
            
            return Enumerable
                .Range(1, DateTime.DaysInMonth(instanceDateTime.Year, instanceDateTime.Month))
                .Select(dayValue => new DateTime(instanceDateTime.Year, instanceDateTime.Month, dayValue))
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
