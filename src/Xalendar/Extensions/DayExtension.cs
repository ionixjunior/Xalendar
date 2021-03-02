using System;
using Xalendar.Api.Interfaces;
using Xalendar.Api.Models;

namespace Xalendar.Extensions
{
    public static class DayExtension
    {
        public static void AddEvent(this Day day, ICalendarViewEvent @event)
        {
            day.Events.Add(@event);
        }

        public static void RemoveEvent(this Day day, ICalendarViewEvent @event)
        {
            day.Events.Remove(@event);
        }

        public static void RemoveAllEvents(this Day day)
        {
            day.Events.Clear();
        }
    }
}
