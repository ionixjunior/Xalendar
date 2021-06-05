using System;
using System.Collections.ObjectModel;
using Xalendar.Api.Interfaces;
using Xalendar.Sample.Models;

namespace Xalendar.Sample.ViewModels
{
    public class AddingEventsViewModel
    {
        public ObservableCollection<CustomEvent> Events { get; }

        public AddingEventsViewModel()
        {
            Events = new ObservableCollection<CustomEvent>();

            var today = DateTime.Today;
            var tomorrow = DateTime.Today.AddDays(1);
            var yesterday = DateTime.Today.AddDays(-1);

            var eventToday = new DateTime(today.Year, today.Month, today.Day);
            Events.Add(new CustomEvent(1, $"Event 1", eventToday, eventToday, false));

            var eventTomorrow = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day);
            Events.Add(new CustomEvent(1, $"Event 1", eventTomorrow, eventTomorrow, false));

            var eventYesterday = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day);
            Events.Add(new CustomEvent(1, $"Event 1", eventYesterday, eventYesterday, false));
        }
    }
}
