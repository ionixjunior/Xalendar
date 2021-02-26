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

        public static void RemoveAllEvents(this MonthContainer monthContainer) =>
            monthContainer._month.RemoveAllEvents();
        
        public static string GetName(this MonthContainer monthContainer) => monthContainer._month.MonthDateTime.ToString("MMMM yyyy");
    }
}
