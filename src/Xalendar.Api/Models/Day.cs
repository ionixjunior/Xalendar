using System;
using System.Collections.Generic;
using System.Linq;
using Xalendar.Api.Interfaces;

namespace Xalendar.Api.Models
{
    public class Day
    {
        private DateTime _currentDateTime;
        public DateTime DateTime { get; }
        public IList<ICalendarViewEvent> Events { get; }

        public bool IsWeekend => DateTime.DayOfWeek switch
        {
            DayOfWeek.Saturday => true,
            DayOfWeek.Sunday => true,
            _ => false
        };
        
        public bool HasEvents => Events.Any();

        public Day(DateTime dateTime, bool isSelected = false) : this(dateTime, DateTime.Now, isSelected)
        {
        }

        public Day(DateTime dateTime, DateTime currentDateTime, bool isSelected = false)
        {
            _currentDateTime = currentDateTime;
            DateTime = dateTime;
            _isSelected = isSelected;
            Events = new List<ICalendarViewEvent>();
        }

        public bool IsToday => _currentDateTime.Date == DateTime.Date;

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => _isSelected = value;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Day dayToCompare)
                return dayToCompare.DateTime.Date.Ticks == DateTime.Date.Ticks;

            return false;
        }

        public override int GetHashCode() => DateTime.Date.Ticks.GetHashCode();

        public override string ToString() => DateTime.Day.ToString();
    }
}
