using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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
            GetDaysToDiscardAtStartOfMonth(daysOfContainer);
            daysOfContainer.AddRange(_month.Days);
            GetDaysToDiscardAtEndOfMonth(daysOfContainer);
            Days = daysOfContainer;
        }

        private void GetDaysToDiscardAtStartOfMonth(List<Day?> daysOfContainer)
        {
            var firstDay = _month.Days.First();
            var numberOfDaysToDiscard = (int) firstDay.DateTime.DayOfWeek;
            
            for (var index = 0; index < numberOfDaysToDiscard; index++)
                daysOfContainer.Add(default(Day));
        }

        private void GetDaysToDiscardAtEndOfMonth(List<Day?> daysOfContainer)
        {
            var lastDay = _month.Days.Last();
            var numberOfDaysToDiscard = 6 - (int) lastDay.DateTime.DayOfWeek;
            
            for (var index = 0; index < numberOfDaysToDiscard; index++)
                daysOfContainer.Add(default(Day));
        }
    }
}
