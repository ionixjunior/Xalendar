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
        
        public IReadOnlyList<Day?> Days { get; }
        
        public MonthContainer(DateTime dateTime)
        {
            _month = new Month(dateTime);
            
            var daysOfContainer = new List<Day?>();
            this.GetDaysToDiscardAtStartOfMonth(daysOfContainer);
            daysOfContainer.AddRange(_month.Days);
            this.GetDaysToDiscardAtEndOfMonth(daysOfContainer);
            Days = daysOfContainer;
        }
    }
}
