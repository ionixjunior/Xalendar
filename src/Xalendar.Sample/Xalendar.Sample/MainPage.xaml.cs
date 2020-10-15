﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xalendar.Api.Interfaces;
using Xamarin.Forms;

namespace Xalendar.Sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }
        
        private void OnRandomButtonClick(object sender, EventArgs e)
        {
            if (BindingContext is MainPageViewModel viewModel)
                viewModel.AddRandomEvent();
        }

        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            if (BindingContext is MainPageViewModel viewModel)
                viewModel.RemoveEvent();
        }

        private void OnMonthChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Mudou de mês");
        }
    }

    public class MainPageViewModel
    {
        public ObservableCollection<ICalendarViewEvent> Events { get; }
        
        public Command RemoveAllEventsCommand { get; }
        public Command ReplaceEventCommand { get; }
    
        public MainPageViewModel()
        {
            Events = new ObservableCollection<ICalendarViewEvent>();

            RemoveAllEventsCommand = new Command(RemoveAllEvents);
            ReplaceEventCommand = new Command(ReplaceEvent);
    
            for (var index = 1; index <= 10; index++)
            {
                var eventDate = new DateTime(2020, 9, index);
                Events.Add(new CustomEvent(index, "Nome evento", eventDate, eventDate, false));
            }
        }

        private void RemoveAllEvents() => Events.Clear();

        private void ReplaceEvent()
        {
            var firstEvent = Events.FirstOrDefault();
            
            if (firstEvent is null)
                return;
            
            var eventDate = new DateTime(2020, 9, 24);
            var newEvent = new CustomEvent(firstEvent.Id, firstEvent.Name, eventDate, eventDate, firstEvent.IsAllDay);
            Events[0] = newEvent;
        }

        private int _dayEventToStart = 11;
        
        public void AddRandomEvent()
        {
            try
            {
                var eventDate = new DateTime(2020, 9, _dayEventToStart);
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
