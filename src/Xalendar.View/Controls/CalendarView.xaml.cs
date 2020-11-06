using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Xalendar.Api.Extensions;
using Xalendar.Api.Interfaces;
using Xalendar.Api.Models;
using Xalendar.View.Extensions;
using Xamarin.Forms;
using XView = Xamarin.Forms.View;

namespace Xalendar.View.Controls
{
    public partial class CalendarView : ContentView
    {
        public static BindableProperty EventsProperty =
            BindableProperty.Create(
                nameof(Events),
                typeof(IEnumerable<ICalendarViewEvent>),
                typeof(CalendarView),
                null,
                BindingMode.OneWay,
                propertyChanged: OnEventsChanged);
        
        public IEnumerable<ICalendarViewEvent> Events
        {
            get => (IEnumerable<ICalendarViewEvent>)GetValue(EventsProperty);
            set => SetValue(EventsProperty, value);
        }
        
        private static void OnEventsChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is CalendarView calendarView)
            {
                if (oldvalue is INotifyCollectionChanged oldEvents)
                    oldEvents.CollectionChanged -= OnEventsCollectionChanged;
                    
                if (newvalue is INotifyCollectionChanged newEvents)
                    newEvents.CollectionChanged += OnEventsCollectionChanged;

                if (newvalue is IEnumerable<ICalendarViewEvent> events)
                    AddEvents(calendarView, events);
                
                void OnEventsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
                {
                    if (args.Action == NotifyCollectionChangedAction.Add)
                    {
                        var notifiedEvents = args.NewItems.Cast<ICalendarViewEvent>();
                        AddEvents(calendarView, notifiedEvents);
                    }

                    if (args.Action == NotifyCollectionChangedAction.Remove)
                    {
                        var notifiedEvents = args.OldItems.Cast<ICalendarViewEvent>();
                        RemoveEvents(calendarView, notifiedEvents);
                    }

                    if (args.Action == NotifyCollectionChangedAction.Reset)
                        RemoveAllEvents(calendarView);

                    if (args.Action == NotifyCollectionChangedAction.Replace)
                    {
                        var oldEventsNotified = args.OldItems.Cast<ICalendarViewEvent>();
                        var newEventsNotified = args.NewItems.Cast<ICalendarViewEvent>();
                        ReplaceEvents(calendarView, oldEventsNotified, newEventsNotified);
                    }
                }
            }
        }

        private static void AddEvents(CalendarView calendarView, IEnumerable<ICalendarViewEvent> events)
        {
            if (calendarView._monthContainer is null)
                return;
            
            calendarView._monthContainer.AddEvents(events);
            calendarView.RecycleDays(calendarView._monthContainer.Days);
        }

        private static void RemoveEvents(CalendarView calendarView, IEnumerable<ICalendarViewEvent> notifiedEvents)
        {
            foreach (var calendarViewEvent in notifiedEvents)
                calendarView._monthContainer.RemoveEvent(calendarViewEvent);
            
            calendarView.RecycleDays(calendarView._monthContainer.Days);
        }

        private static void RemoveAllEvents(CalendarView calendarView)
        {
            calendarView._monthContainer.RemoveAllEvents();
            calendarView.RecycleDays(calendarView._monthContainer.Days);
        }

        private static void ReplaceEvents(CalendarView calendarView, IEnumerable<ICalendarViewEvent> oldEventsNotified,
            IEnumerable<ICalendarViewEvent> newEventsNotified)
        {
            RemoveEvents(calendarView, oldEventsNotified);
            AddEvents(calendarView, newEventsNotified);
            calendarView.RecycleDays(calendarView._monthContainer.Days);
        }
        
        public static BindableProperty FirstDayOfWeekProperty =
            BindableProperty.Create(
                nameof(FirstDayOfWeek),
                typeof(DayOfWeek),
                typeof(CalendarView),
                DayOfWeek.Sunday,
                BindingMode.OneTime);
        
        public DayOfWeek FirstDayOfWeek
        {
            get => (DayOfWeek)GetValue(FirstDayOfWeekProperty);
            set => SetValue(FirstDayOfWeekProperty, value);
        }

        public event Action<MonthRange>? MonthChanged;

        private MonthContainer _monthContainer;
        private int _numberOfDaysInContainer;

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "Renderer")
            {
                _monthContainer = new MonthContainer(DateTime.Today, FirstDayOfWeek);
                
                if (!Events.IsNullOrEmpty())
                    _monthContainer.AddEvents(Events);

                var days = _monthContainer.Days;
                _numberOfDaysInContainer = days.Count;
                foreach (var _ in days)
                    CalendarDaysContainer.Children.Add(new CalendarDay());
                RecycleDays(days);
                
                BindableLayout.SetItemsSource(CalendarDaysOfWeekContainer, _monthContainer.DaysOfWeek);
                MonthName.Text = _monthContainer.GetName();
                MonthChanged?.Invoke(new MonthRange(_monthContainer.FirstDay, _monthContainer.LastDay));
            }
        }

        public CalendarView()
        {
            InitializeComponent();
        }

        private async void OnPreviousMonthClick(object sender, EventArgs e)
        {
            var result = await Task.Run(() =>
            {
                _monthContainer.Previous();
                
                var days = _monthContainer.Days;
                var monthName = _monthContainer.GetName();
                var firstDay = _monthContainer.FirstDay;
                var lastDay = _monthContainer.LastDay;

                return (days, monthName, firstDay, lastDay);
            });

            MonthName.Text = result.monthName;
            RecycleDays(result.days);
            MonthChanged?.Invoke(new MonthRange(result.firstDay, result.lastDay));
        }

        private async void OnNextMonthClick(object sender, EventArgs e)
        {
            var result = await Task.Run(() =>
            {
                _monthContainer.Next();
                
                var days = _monthContainer.Days;
                var monthName = _monthContainer.GetName();
                var firstDay = _monthContainer.FirstDay;
                var lastDay = _monthContainer.LastDay;

                return (days, monthName, firstDay, lastDay);
            });
            
            MonthName.Text = result.monthName;
            RecycleDays(result.days);
            MonthChanged?.Invoke(new MonthRange(result.firstDay, result.lastDay));
        }

        private void RecycleDays(IReadOnlyList<Day?> days)
        {
            for (var index = 0; index < _numberOfDaysInContainer; index++)
            {
                var day = days[index];
                var view = CalendarDaysContainer.Children[index];

                if (view.FindByName<XView>("HasEventsElement") is {} hasEventsElement)
                    hasEventsElement.IsVisible = day?.HasEvents ?? false;

                if (view.FindByName<XView>("DayContainer") is {} dayContainer)
                    dayContainer.BackgroundColor = day is {} && day.IsToday ? Color.Red : Color.Transparent;

                if (view.FindByName<Label>("DayElement") is {} dayElement)
                    dayElement.Text = day?.ToString();
            }
        }
    }

    public readonly struct MonthRange
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        public MonthRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
    }
}

