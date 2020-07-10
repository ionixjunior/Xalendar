using System.Collections.Generic;
using Xalendar.Api.Models;

namespace Xalendar.Api.Extensions
{
    public static class MonthContainerExtension
    {
        public static void SelectDay(this MonthContainer monthContainer, Day selectedDay) => monthContainer._month.SelectDay(selectedDay);
        public static Day GetSelectedDay(this MonthContainer monthContainer) => monthContainer._month.GetSelectedDay();
        public static void AddEvents(this MonthContainer monthContainer, IList<Event> events) => monthContainer._month.AddEvents(events);
        public static string GetName(this MonthContainer monthContainer) => monthContainer._month.MonthDateTime.ToString("MMMM");

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
    }
}
