using System;
using System.Collections.Generic;
using Xalendar.Api.Enums;
using Xalendar.Api.Interfaces;

namespace Xalendar.Api.Models
{
    public struct DayTapped
    {
        public DateTime DateTime { get; }
        public IEnumerable<ICalendarViewEvent> Events { get; }
        public DayState State { get; }

        public DayTapped(DateTime dateTime, IEnumerable<ICalendarViewEvent> events, DayState state)
        {
            DateTime = dateTime;
            Events = events;
            State = state;
        }
    }
}
