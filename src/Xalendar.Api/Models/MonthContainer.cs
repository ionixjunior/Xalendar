using System;
using System.Collections.Generic;
using System.ComponentModel;

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
    }
}
