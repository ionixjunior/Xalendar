using System;

namespace Xalendar.Api.Models
{
    public class Day
    {
        private readonly DateTime _dateTime;

        public Day(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public bool IsToday => DateTime.Now.Day == _dateTime.Day;
        
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => _isSelected = value;
        }

        public DateTime DateTime => _dateTime;
    }
}
