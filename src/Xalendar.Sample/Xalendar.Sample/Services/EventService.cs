using System;
using System.Collections.Generic;
using System.Linq;
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
                var dateTime = DateTime.Today;
                var eventDate = new DateTime(dateTime.Year, dateTime.Month, index);
                _events.Add(new CustomEvent(index, "Nome evento", eventDate, eventDate, false));
            }
            
            for (var index = 3; index <= 8; index++)
            {
                var dateTime = DateTime.Today.AddMonths(1);
                var eventDate = new DateTime(dateTime.Year, dateTime.Month, index);
                _events.Add(new CustomEvent(index, "Nome evento", eventDate, eventDate, false));
            }
            
            for (var index = 5; index <= 9; index++)
            {
                var dateTime = DateTime.Today.AddMonths(2);
                var eventDate = new DateTime(dateTime.Year, dateTime.Month, index);
                _events.Add(new CustomEvent(index, "Nome evento", eventDate, eventDate, false));
            }
            
            for (var index = 15; index <= 16; index++)
            {
                var dateTime = DateTime.Today.AddMonths(3);
                var eventDate = new DateTime(dateTime.Year, dateTime.Month, index);
                _events.Add(new CustomEvent(index, "Nome evento", eventDate, eventDate, false));
            }
            
            for (var index = 6; index <= 10; index++)
            {
                var dateTime = DateTime.Today.AddMonths(-1);
                var eventDate = new DateTime(dateTime.Year, dateTime.Month, index);
                _events.Add(new CustomEvent(index, "Nome evento", eventDate, eventDate, false));
            }
            
            for (var index = 2; index <= 5; index++)
            {
                var dateTime = DateTime.Today.AddMonths(-2);
                var eventDate = new DateTime(dateTime.Year, dateTime.Month, index);
                _events.Add(new CustomEvent(index, "Nome evento", eventDate, eventDate, false));
            }
        }

        public IList<ICalendarViewEvent> GetEvents() => _events;

        public IList<ICalendarViewEvent> GetEventsByRange(DateTime start, DateTime end) =>
            _events.Where(x => x.StartDateTime >= start)
                .Where(x => x.EndDateTime <= end)
                .ToList();
    }
}
