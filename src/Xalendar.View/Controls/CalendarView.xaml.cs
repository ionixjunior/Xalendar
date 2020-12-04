using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
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
            if (calendarView._lazyContainer.IsNull())
                return;

            calendarView._lazyContainer.Set(m => m.AddEvents(events));
            calendarView.RecycleDays(calendarView._lazyContainer.Get(m => m.Days));
        }

        private static void RemoveEvents(CalendarView calendarView, IEnumerable<ICalendarViewEvent> notifiedEvents)
        {
            if (calendarView._lazyContainer.IsNull())
                return;

            foreach (var calendarViewEvent in notifiedEvents)
                calendarView._lazyContainer.Set(m => m.RemoveEvent(calendarViewEvent));

            calendarView.RecycleDays(calendarView._lazyContainer.Get(m => m.Days));
        }

        private static void RemoveAllEvents(CalendarView calendarView)
        {
            if (calendarView._lazyContainer.IsNull())
                return;

            calendarView._lazyContainer.Set(m => m.RemoveAllEvents());
            calendarView.RecycleDays(calendarView._lazyContainer.Get(m => m.Days));
        }

        private static void ReplaceEvents(CalendarView calendarView, IEnumerable<ICalendarViewEvent> oldEventsNotified,
            IEnumerable<ICalendarViewEvent> newEventsNotified)
        {
            if (calendarView._lazyContainer.IsNull())
                return;

            RemoveEvents(calendarView, oldEventsNotified);
            AddEvents(calendarView, newEventsNotified);
            calendarView.RecycleDays(calendarView._lazyContainer.Get(m => m.Days));
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
        public event Action<DaySelected>? DaySelected;

        //Agora pode ser readonly
        private readonly LazyMonthContainer _lazyContainer;
        private int _numberOfDaysInContainer;

        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "Renderer")
            {
                //Com esse encapsulamento da pra criar uma sintaxe fluente (apenas demo nao repassei o resto do codigo)
                var days = _lazyContainer.Build()
                    .AddEvents(Events)
                    .Get(m => m.Days);

                _numberOfDaysInContainer = days.Count;
                foreach (var _ in days)
                {
                    var calendarDay = new CalendarDay();
                    calendarDay.DaySelected += CalendarDayOnDaySelected;
                    calendarDay.UnSelect();
                    CalendarDaysContainer.Children.Add(calendarDay);
                }

                RecycleDays(days);

                _lazyContainer.Set(m =>
                    BindableLayout.SetItemsSource(CalendarDaysOfWeekContainer, m.DaysOfWeek));

                MonthName.Text = _lazyContainer.Get(m => m.GetName());
                MonthChanged?.Invoke(new MonthRange(_lazyContainer.Get(m => m.FirstDay), _lazyContainer.Get(m => m.LastDay)));
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
            _lazyContainer = new LazyMonthContainer(() => FirstDayOfWeek);
            InitializeComponent();
        }

        private async void OnPreviousMonthClick(object sender, EventArgs e)
        {
            _selectedDay?.UnSelect();
            _selectedDay = null;

            var result = await Task.Run(() =>
            {
                if (_lazyContainer.IsNull())
                    return default;

                _lazyContainer.Set(m => m.Previous());

                var days = _lazyContainer.Get(m => m.Days);
                var monthName = _lazyContainer.Get(m => m.GetName());
                var firstDay = _lazyContainer.Get(m => m.FirstDay);
                var lastDay = _lazyContainer.Get(m => m.LastDay);

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
                if (_lazyContainer.IsNull())
                    return default;

                _lazyContainer.Set(m => m.Next());

                var days = _lazyContainer.Get(m => m.Days);
                var monthName = _lazyContainer.Get(m => m.GetName());
                var firstDay = _lazyContainer.Get(m => m.FirstDay);
                var lastDay = _lazyContainer.Get(m => m.LastDay);

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

                    if (view.FindByName<XView>("HasEventsElement") is { } hasEventsElement)
                        hasEventsElement.IsVisible = day?.HasEvents ?? false;

                    if (view.FindByName<XView>("DayContainer") is { } dayContainer)
                        dayContainer.BackgroundColor = day is { } && day.IsToday ? Color.Red : Color.Transparent;

                    if (view.FindByName<Label>("DayElement") is { } dayElement)
                        dayElement.Text = day?.ToString();
                }

            }
        }

        /// <summary>
        /// Encapsula o acesso ao MonthContainer, deixa thread safe
        /// e deixa o month container com inicialização tardia
        /// </summary>
        private readonly struct LazyMonthContainer
        {
            private readonly Lazy<MonthContainer> _monthContainer;

            public LazyMonthContainer(Func<DayOfWeek> dayOfWeekGetter)
            {
                _monthContainer = new Lazy<MonthContainer>(() =>
                        new MonthContainer(DateTime.Today, dayOfWeekGetter()), true/*vantagem que podemos fazer o acesso ao membro thread safe*/);
            }

            /// Se não gostar do Set/Get da pra usar:
            /// public MonthContainer GetContainer()
            /// {
            ///     return _monthContainer.Value;
            /// }
            public LazyMonthContainer Set(Action<MonthContainer> setter)
            {
                if (_monthContainer.IsValueCreated)
                {
                    setter.Invoke(_monthContainer.Value);
                }

                return this;
            }

            public TResult Get<TResult>(Func<MonthContainer, TResult> getter) =>
                getter.Invoke(_monthContainer.Value);

            public LazyMonthContainer AddEvents(IEnumerable<ICalendarViewEvent> events)
            {
                if (!events.IsNullOrEmpty())
                {
                    Set(m => m.AddEvents(events));
                }

                return this;
            }

            public LazyMonthContainer Build()
            {
                _ = _monthContainer.Value;

                return this;
            }

            public bool IsNull() =>
                _monthContainer.IsValueCreated != true;
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

