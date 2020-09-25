using System.Collections.Generic;
using System.Linq;
using Xalendar.Api.Interfaces;
using Xalendar.Api.Models;

namespace Xalendar.Api.Extensions
{
    public static class MonthContainerExtension
    {
        public static void SelectDay(this MonthContainer monthContainer, Day selectedDay) => monthContainer._month.SelectDay(selectedDay);
        public static Day GetSelectedDay(this MonthContainer monthContainer) => monthContainer._month.GetSelectedDay();
        public static void AddEvents(this MonthContainer monthContainer, IEnumerable<ICalendarViewEvent> events) => monthContainer._month.AddEvents(events);

        public static void RemoveEvent(this MonthContainer monthContainer, ICalendarViewEvent calendarViewEvent) =>
            monthContainer._month.RemoveEvent(calendarViewEvent);
        
        public static string GetName(this MonthContainer monthContainer) => monthContainer._month.MonthDateTime.ToString("MMMM yyyy");

        public static void Next(this MonthContainer monthContainer)
        {
            var nextDateTime = monthContainer._month.MonthDateTime.AddMonths(1);
            monthContainer._month = new Month(nextDateTime);
        }

        public static void Previous(this MonthContainer monthContainer)
        {
            var previousDateTime = monthContainer._month.MonthDateTime.AddMonths(-1);
            monthContainer._month = new Month(previousDateTime);
        }
        
        internal static void GetDaysToDiscardAtStartOfMonth(this MonthContainer monthContainer, List<Day?> daysOfContainer)
        {
            var firstDay = monthContainer._month.Days.First();
            var numberOfDaysToDiscard = (int) firstDay!.DateTime.DayOfWeek;
            
            for (var index = 0; index < numberOfDaysToDiscard; index++)
                daysOfContainer.Add(default(Day));
        }
        
        internal static void GetDaysToDiscardAtEndOfMonth(this MonthContainer monthContainer, List<Day?> daysOfContainer)
        {
            var lastDay = monthContainer._month.Days.Last();
            var numberOfDaysToDiscard = 6 - (int) lastDay.DateTime.DayOfWeek;
            
            for (var index = 0; index < numberOfDaysToDiscard; index++)
                daysOfContainer.Add(default(Day));

            if (daysOfContainer.Count < 42)
                for (var index = daysOfContainer.Count; index < 42; index++)
                    daysOfContainer.Add(default(Day));
        }
    }
}
