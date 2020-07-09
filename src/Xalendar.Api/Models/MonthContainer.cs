using System;
using System.Collections.Generic;
using Xalendar.Api.Extensions;

namespace Xalendar.Api.Models
{
    public class MonthContainer
    {
        private Month _month;
        
        public IReadOnlyList<Day> Days { get; }
        
        public MonthContainer(DateTime dateTime)
        {
            _month = new Month(dateTime);
            Days = _month.Days;
        }

        public void SelectDay(Day selectedDay) => _month.SelectDay(selectedDay);
        public Day GetSelectedDay() => _month.GetSelectedDay();
    }
}
