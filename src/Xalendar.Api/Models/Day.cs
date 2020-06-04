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
    }
}
