using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xalendar.Api.Interfaces;
using Xalendar.Sample.Models;
using Xalendar.Sample.Services;
using Xamarin.Forms;

namespace Xalendar.Sample.ViewModels
{
    public class MainPageViewModel
    {
        public ObservableCollection<ICalendarViewEvent> Events { get; }
        public ObservableCollection<ICalendarViewEvent> EventsOfDay { get; }
        
        public Command RemoveAllEventsCommand { get; }
        public Command ReplaceEventCommand { get; }
    
        public MainPageViewModel()
        {
            Events = new ObservableCollection<ICalendarViewEvent>();
            EventsOfDay = new ObservableCollection<ICalendarViewEvent>();

            RemoveAllEventsCommand = new Command(RemoveAllEvents);
            ReplaceEventCommand = new Command(ReplaceEvent);
        }

        private void RemoveAllEvents() => Events.Clear();

        private void ReplaceEvent()
        {
            var firstEvent = Events.FirstOrDefault();
            
            if (firstEvent is null)
                return;
            
            var eventDate = new DateTime(2020, 10, 24);
            var newEvent = new CustomEvent(firstEvent.Id, firstEvent.Name, eventDate, eventDate, firstEvent.IsAllDay);
            Events[0] = newEvent;
        }

        private int _dayEventToStart = 11;
        
        public void AddRandomEvent()
        {
            try
            {
                var eventDate = new DateTime(2020, 10, _dayEventToStart);
                var customEvent = new CustomEvent(_dayEventToStart, "Nome evento", eventDate, eventDate, false);
                Events.Add(customEvent);
                _dayEventToStart++;
            }
            catch (Exception) { }
        }

        public void RemoveEvent()
        {
            var firstEvent = Events.FirstOrDefault();

            if (firstEvent != null)
                Events.Remove(firstEvent);
        }

        public void GetEventsByRange(DateTime start, DateTime end)
        {
            Events.Clear();
            
            foreach (var customEvent in EventService.Instance.GetEventsByRange(start, end))
                Events.Add(customEvent);
        }

        public void AddEventsOfDay(IEnumerable<ICalendarViewEvent> events)
        {
            EventsOfDay.Clear();
            
            foreach (var calendarEvent in events)
                EventsOfDay.Add(calendarEvent);
        }
    }
}
