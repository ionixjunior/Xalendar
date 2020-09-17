using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xalendar.Api.Interfaces;
using Xalendar.Api.Models;
using Xamarin.Forms;

namespace Xalendar.Sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var events = new List<ICalendarViewEvent>();

            for (var index = 1; index <= 10; index++)
            {
                var eventDate = new DateTime(2020, 9, index);
                events.Add(new CustomEvent(index, "Nome evento", eventDate, eventDate, false));
            }
            
            Calendar.Events = events;
        }
    }

    public class CustomEvent : ICalendarViewEvent
    {
        public object Id { get; }
        public string Name { get; }
        public DateTime StartDateTime { get; }
        public DateTime EndDateTime { get; }
        public bool IsAllDay { get; }
        
        public CustomEvent(object id, string name, DateTime startDateTime, DateTime endDateTime, bool isAllDay)
        {
            Id = id;
            Name = name;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            IsAllDay = isAllDay;
        }
    }
}
