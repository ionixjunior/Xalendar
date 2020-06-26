using System.Collections.Generic;
using Xalendar.Api.Models;

namespace Xalendar.Api.Extensions
{
    public static class DayExtension
    {
        public static void AddEvents(this Day day, IList<Event> events)
        {
            foreach (var @event in events)
                day.Events.Add(@event);
        }
    }
}
