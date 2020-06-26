using System.Collections.Generic;
using Xalendar.Api.Models;

namespace Xalendar.Api.Extensions
{
    public static class DayExtension
    {
        public static void AddEvent(this Day day, Event @event)
        {
            day.Events.Add(@event);
        }
    }
}
