using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xalendar.Api.Extensions;

namespace Xalendar.Api.Models
{
    public class MonthContainer
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Month _month;
        
        public IReadOnlyList<Day> Days { get; }
        
        public MonthContainer(DateTime dateTime)
        {
            _month = new Month(dateTime);
            Days = _month.Days;
        }

        public void SelectDay(Day selectedDay) => _month.SelectDay(selectedDay);
        public Day GetSelectedDay() => _month.GetSelectedDay();
        public void AddEvents(IList<Event> events) => _month.AddEvents(events);
        public string GetName() => _month.MonthDateTime.ToString("MMMM");

        public void Next()
        {
            var nextDateTime = _month.MonthDateTime.AddMonths(1);
            _month = new Month(nextDateTime);
        }

        public void Previous()
        {
            var previousDateTime = _month.MonthDateTime.AddMonths(-1);
            _month = new Month(previousDateTime);
        }
    }
}
