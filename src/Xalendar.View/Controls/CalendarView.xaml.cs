using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xalendar.Api.Extensions;
using Xalendar.Api.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xalendar.View.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarView : ContentView
    {
        public static BindableProperty EventsProperty =
            BindableProperty.Create(
                nameof(Events),
                typeof(IList<Event>),
                typeof(CalendarView),
                null,
                BindingMode.OneWay,
                propertyChanged: OnEventsChanged);
        
        public IList<Event> Events
        {
            get => (IList<Event>)GetValue(EventsProperty);
            set => SetValue(EventsProperty, value);
        }
        
        private static void OnEventsChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is CalendarView calendarView && newvalue is IList<Event> events)
            {
                calendarView._monthContainer.AddEvents(events);
                calendarView.RecycleDays(calendarView._monthContainer.Days);
            }
        }
        
        private readonly MonthContainer _monthContainer;
        private readonly int _numberOfDaysInContainer;
        
        public CalendarView()
        {
            InitializeComponent();
            
            _monthContainer = new MonthContainer(DateTime.Today);
            BindableLayout.SetItemsSource(CalendarDaysContainer, _monthContainer.Days);
            BindableLayout.SetItemsSource(CalendarDaysOfWeekContainer, _monthContainer.DaysOfWeek);
            MonthName.Text = _monthContainer.GetName();
            _numberOfDaysInContainer = CalendarDaysContainer.Children.Count;
        }

        private async void OnPreviousMonthClick(object sender, EventArgs e)
        {
            var result = await Task.Run(() =>
            {
                _monthContainer.Previous();
                
                var days = _monthContainer.Days;
                var monthName = _monthContainer.GetName();

                return (days, monthName);
            });

            MonthName.Text = result.monthName;
            RecycleDays(result.days);
        }

        private async void OnNextMonthClick(object sender, EventArgs e)
        {
            var result = await Task.Run(() =>
            {
                _monthContainer.Next();
                
                var days = _monthContainer.Days;
                var monthName = _monthContainer.GetName();

                return (days, monthName);
            });
            
            MonthName.Text = result.monthName;
            RecycleDays(result.days);
        }

        private void RecycleDays(IReadOnlyList<Day?> days)
        {
            for (var index = 0; index < _numberOfDaysInContainer; index++)
            {
                var day = days[index];
                var view = CalendarDaysContainer.Children[index];
                view.BindingContext = day;

                if (view.FindByName<BoxView>("HasEventsElement") is {} hasEventsElement)
                    hasEventsElement.IsVisible = day?.HasEvents ?? false;
            }
        }
    }
}

