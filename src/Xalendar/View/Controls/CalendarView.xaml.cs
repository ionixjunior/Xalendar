using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xalendar.Extensions;
using Xalendar.Api.Interfaces;
using Xalendar.Api.Models;
using Xamarin.Forms;
using XView = Xamarin.Forms.View;
using Xalendar.Api.Formatters;

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
            if (calendarView._monthContainer is null)
                return;
            
            foreach (var calendarViewEvent in notifiedEvents)
                calendarView._monthContainer.RemoveEvent(calendarViewEvent);
            
            calendarView.RecycleDays(calendarView._monthContainer.Days);
        }

        private static void RemoveAllEvents(CalendarView calendarView)
        {
            if (calendarView._monthContainer is null)
                return;
            
            calendarView._monthContainer.RemoveAllEvents();
            calendarView.RecycleDays(calendarView._monthContainer.Days);
        }

        private static void ReplaceEvents(CalendarView calendarView, IEnumerable<ICalendarViewEvent> oldEventsNotified,
            IEnumerable<ICalendarViewEvent> newEventsNotified)
        {
            if (calendarView._monthContainer is null)
                return;
            
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

        public static BindableProperty IsPreviewDaysActiveProperty =
            BindableProperty.Create(
                nameof(IsPreviewDaysActive),
                typeof(bool),
                typeof(CalendarView),
                false,
                BindingMode.OneTime);

        public bool IsPreviewDaysActive
        {
            get => (bool)GetValue(IsPreviewDaysActiveProperty);
            set => SetValue(IsPreviewDaysActiveProperty, value);
        }

        public static BindableProperty DaysOfWeekFormatterProperty =
            BindableProperty.Create(
                nameof(DaysOfWeekFormatter),
                typeof(IDayOfWeekFormatter),
                typeof(CalendarView),
                new DayOfWeek3CaractersFormat(),
                BindingMode.OneTime);

        public IDayOfWeekFormatter DaysOfWeekFormatter
        {
            get => (IDayOfWeekFormatter)GetValue(DaysOfWeekFormatterProperty);
            set => SetValue(DaysOfWeekFormatterProperty, value);
        }

        public event Action<MonthRange>? MonthChanged;
        public event Action<DaySelected>? DaySelected;

        private MonthContainer? _monthContainer;
        private int _numberOfDaysInContainer;

        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "Renderer")
            {
                _monthContainer = new MonthContainer(DateTime.Today, DaysOfWeekFormatter, FirstDayOfWeek, IsPreviewDaysActive);
                
                if (!Events.IsNullOrEmpty())
                    _monthContainer.AddEvents(Events);

                var days = _monthContainer.Days;
                _numberOfDaysInContainer = days.Count;

                var column = 0;
                var row = 0;

                foreach (var _ in days)
                {
                    var calendarDay = new CalendarDay();
                    calendarDay.DaySelected += CalendarDayOnDaySelected;
                    CalendarDaysContainer.Children.Add(calendarDay, column, row);

                    if (++column > 6)
                    {
                        column = 0;
                        row++;
                    }
                }
                RecycleDays(days);

                column = 0;

                foreach (var dayOfWeek in _monthContainer.DaysOfWeek)
                {
                    var calendarDayOfWeek = new CalendarDayOfWeek();
                    CalendarDaysOfWeekContainer.Children.Add(calendarDayOfWeek, column++, 0);
                    calendarDayOfWeek.UpdateData(dayOfWeek);
                }

                MonthName.Text = _monthContainer.GetName();
                MonthChanged?.Invoke(new MonthRange(_monthContainer.FirstDay, _monthContainer.LastDay));
            }
        }

        private CalendarDay? _selectedDay;

        private void CalendarDayOnDaySelected(CalendarDay? calendarDay)
        {
            if (_selectedDay == calendarDay)
                return;

            if (calendarDay?.Day is null)
                return;
            
            _selectedDay?.UnSelect();
            calendarDay.Select();
            _selectedDay = calendarDay;
            DaySelected?.Invoke(new DaySelected(calendarDay.Day.DateTime, calendarDay.Day.Events));
        }

        public CalendarView()
        {
            InitializeComponent();
        }

        private async void OnPreviousMonthClick(object sender, EventArgs e)
        {
            _selectedDay?.UnSelect();
            _selectedDay = null;
            
            var result = await Task.Run(() =>
            {
                if (_monthContainer is null)
                    return default;
                
                _monthContainer.Previous();
                
                var days = _monthContainer.Days;
                var monthName = _monthContainer.GetName();
                var firstDay = _monthContainer.FirstDay;
                var lastDay = _monthContainer.LastDay;

                return (days, monthName, firstDay, lastDay);
            });

            if (result.Equals(default))
                return;

            MonthName.Text = result.monthName;
            RecycleDays(result.days);
            MonthChanged?.Invoke(new MonthRange(result.firstDay, result.lastDay));
        }

        private async void OnNextMonthClick(object sender, EventArgs e)
        {
            _selectedDay?.UnSelect();
            _selectedDay = null;
            
            var result = await Task.Run(() =>
            {
                if (_monthContainer is null)
                    return default;
                
                _monthContainer.Next();
                
                var days = _monthContainer.Days;
                var monthName = _monthContainer.GetName();
                var firstDay = _monthContainer.FirstDay;
                var lastDay = _monthContainer.LastDay;

                return (days, monthName, firstDay, lastDay);
            });
            
            if (result.Equals(default))
                return;
            
            MonthName.Text = result.monthName;
            RecycleDays(result.days);
            MonthChanged?.Invoke(new MonthRange(result.firstDay, result.lastDay));
        }

        private void RecycleDays(IReadOnlyList<Day?> days)
        {
            for (var index = 0; index < _numberOfDaysInContainer; index++)
            {
                var day = days[index];

                if (CalendarDaysContainer.Children[index] is CalendarDay view)
                {
                    view.Day = day;
                    
                    if (view.FindByName<XView>("HasEventsElement") is {} hasEventsElement)
                        hasEventsElement.IsVisible = day?.HasEvents ?? false;

                    if (view.FindByName<CalendarDay>("DayContainer") is { } dayContainer)
                        dayContainer.StartState();

                    if (view.FindByName<Label>("DayElement") is {} dayElement)
                        dayElement.Text = day?.ToString();
                }

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

    public readonly struct DaySelected
    {
        public DateTime DateTime { get; }
        public IEnumerable<ICalendarViewEvent> Events { get; }

        public DaySelected(DateTime dateTime, IEnumerable<ICalendarViewEvent> events)
        {
            DateTime = dateTime;
            Events = events;
        }
    }
}

