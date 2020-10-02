using System;
using System.Collections.Generic;
using Xalendar.Api.Interfaces;
using Xalendar.Sample.Models;

namespace Xalendar.Sample.Services
{
    public class EventService
    {
        private static EventService _instance;
        public static EventService Instance => _instance ??= new EventService();

        private IList<ICalendarViewEvent> _events;

        private EventService()
        {
            _events = new List<ICalendarViewEvent>();
            
            for (var index = 1; index <= 10; index++)
            {
                var eventDate = new DateTime(2020, 10, index);
                _events.Add(new CustomEvent(index, "Nome evento", eventDate, eventDate, false));
            }
        }

        public IList<ICalendarViewEvent> GetEvents() => _events;
    }
}
