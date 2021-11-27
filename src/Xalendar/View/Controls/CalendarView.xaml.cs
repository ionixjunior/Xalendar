using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xalendar.Api.Enums;
using Xalendar.Extensions;
using Xalendar.Api.Interfaces;
using Xalendar.Api.Models;
using Xamarin.Forms;
using XView = Xamarin.Forms.View;
using Xalendar.Api.Formatters;
using Xalendar.View.Themes;

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

        public static BindableProperty ThemeProperty =
            BindableProperty.Create(
                nameof(Theme),
                typeof(ResourceDictionary),
                typeof(CalendarView),
                new Classic(),
                BindingMode.OneTime);

        public ResourceDictionary Theme
        {
            get => (ResourceDictionary)GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }

        public static BindableProperty SelectModeProperty =
            BindableProperty.Create(
                nameof(SelectMode),
                typeof(SelectMode),
                typeof(CalendarView),
                SelectMode.Single,
                BindingMode.OneTime);

        public SelectMode SelectMode
        {
            get => (SelectMode)GetValue(SelectModeProperty);
            set => SetValue(SelectModeProperty, value);
        }

        public event Action<MonthRange>? MonthChanged;
        [Obsolete("Use DayTapped instead of this one")]
        public event Action<DaySelected>? DaySelected;
        public event Action<DayTapped>? DayTapped;

        private MonthContainer? _monthContainer;
        private int _numberOfDaysInContainer;

        private List<DateTime> _selectedDates = new List<DateTime>();

        public IReadOnlyList<DateTime> SelectedDates
        {
            get => _selectedDates.OrderBy(x => x.Date).ToList();
        }

        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "Renderer")
            {
                Resources.MergedDictionaries.Add(Theme);
                _monthContainer = new MonthContainer(DateTime.Today, dayOfWeekFormatter: DaysOfWeekFormatter, firstDayOfWeek: FirstDayOfWeek, isPreviewDaysActive: IsPreviewDaysActive);
                
                if (!Events.IsNullOrEmpty())
                    _monthContainer.AddEvents(Events);

                var days = _monthContainer.Days;
                _numberOfDaysInContainer = days.Count;

                var column = 0;
                var row = 0;

                foreach (var _ in days)
                {
                    var calendarDay = new CalendarDay();
                    calendarDay.DayTapped += CalendarDayOnDayTapped;
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

        private CalendarDay? _lastSelectedDay;

        private void CalendarDayOnDaySelected(CalendarDay? calendarDay)
        {
            if (_lastSelectedDay == calendarDay)
                return;

            if (calendarDay?.Day is null)
                return;
            
            UnSelectLastSelectedDay();
            calendarDay.Select();
            _lastSelectedDay = calendarDay;
            DaySelected?.Invoke(new DaySelected(calendarDay.Day.DateTime, calendarDay.Day.Events));
        }
        
        private void CalendarDayOnDayTapped(CalendarDay? calendarDay)
        {
            if (calendarDay?.Day is null)
                return;

            if (IsUsedNewEvent())
            {
                ChangeDayState(calendarDay);
                var state = calendarDay.Day.IsSelected ? DayState.Selected : DayState.UnSelected;
                UpdateSelectedDates(calendarDay.Day.DateTime, state);
                DayTapped?.Invoke(new DayTapped(calendarDay.Day.DateTime, calendarDay.Day.Events, state));
                return;
            }

            if (IsUsedLegacyEvent())
            {
                CalendarDayOnDaySelected(calendarDay);
                return;
            }
        }

        private bool IsUsedNewEvent() => DayTapped != null;
        private bool IsUsedLegacyEvent() => DaySelected != null;

        private void ChangeDayState(CalendarDay calendarDay)
        {
            if (SelectMode == SelectMode.Single)
                ChangeDayStateForSingleMode(calendarDay);

            if (SelectMode == SelectMode.Multi)
                ChangeDayStateForMultiMode(calendarDay);
        }

        private void ChangeDayStateForSingleMode(CalendarDay calendarDay)
        {
            if (_lastSelectedDay != calendarDay)
                _lastSelectedDay?.UnSelect();
                
            calendarDay.SwitchSelectedState();
            _lastSelectedDay = calendarDay;
        }

        private void ChangeDayStateForMultiMode(CalendarDay calendarDay)
        {
            calendarDay.SwitchSelectedState();
        }

        private void UnSelectLastSelectedDay()
        {
            if (SelectMode == SelectMode.Single)
                _lastSelectedDay?.UnSelect();
        }

        private void UpdateSelectedDates(DateTime dateTime, DayState state)
        {
            if (state == DayState.UnSelected)
            {
                _selectedDates.Remove(dateTime);
                return;
            }

            if (state == DayState.Selected)
            {
                _selectedDates.Add(dateTime);
                return;
            }
        }

        public CalendarView()
        {
            InitializeComponent();
        }

        private async void OnPreviousMonthClick(object sender, EventArgs e)
        {
            _lastSelectedDay?.UnSelect();
            _lastSelectedDay = null;
            
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
            _lastSelectedDay?.UnSelect();
            _lastSelectedDay = null;
            
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
                    var shouldSelect = day is { }
                                       && SelectMode == SelectMode.Multi
                                       && _selectedDates.Contains(day.DateTime);
                    
                    view.UpdateData(day, shouldSelect);
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

