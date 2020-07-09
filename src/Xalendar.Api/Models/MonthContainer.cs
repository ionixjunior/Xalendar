using System;
using System.Collections.Generic;

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
    }
}
