using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using Xalendar.Api.Models;
using Xalendar.View.Controls;
using Xamarin.Forms;

namespace Xalendar.Tests.View.Controls
{
    [TestFixture]
    public class CalendarViewTests
    {
        private CalendarView _calendarView;
        
        [SetUp]
        public void SetUp()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            _calendarView = CreateCalendarViewInstance();
        }

        private CalendarView CreateCalendarViewInstance()
        {
            var calendarViewType = typeof(CalendarView);
            var calendarConstructor = calendarViewType.GetConstructor(Type.EmptyTypes);
            var calendarView = calendarConstructor.Invoke(new object[] { });

            var onPropertyChangedMethod = calendarViewType.GetMethod("OnPropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
            _ = onPropertyChangedMethod.Invoke(calendarView, new object[] {"Renderer"});
            
            return calendarView as CalendarView;
        }

        private void InvokeCalendarViewMethod(string name, object[] parameters)
        {
            var calendarViewType = typeof(CalendarView);
            var method = calendarViewType.GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
            _ = method.Invoke(_calendarView, parameters);
        }

        private DateTime GetStartDateOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0);
        }

        private DateTime GetEndDateOfMonth(DateTime dateTime)
        {
            return new DateTime(
                dateTime.Year, 
                dateTime.Month, 
                DateTime.DaysInMonth(dateTime.Year, dateTime.Month), 
                23, 
                59, 
                59
            );
        }

        [Test]
        public async Task ShouldGetStartDateOfMonthRangeWhenNavigateToPreviousMonth()
        {
            var previousMonth = DateTime.Today.AddMonths(-1);
            var startDate = GetStartDateOfMonth(previousMonth);
            var taskCompletionSource = new TaskCompletionSource<MonthRange>();
            _calendarView.MonthChanged += taskCompletionSource.SetResult;
            
            InvokeCalendarViewMethod("OnPreviousMonthClick", new object[] {null, EventArgs.Empty});
            var range = await taskCompletionSource.Task;
            
            Assert.AreEqual(startDate, range.Start);
        }
        
        [Test]
        public async Task ShouldGetEndDateOfMonthRangeWhenNavigateToPreviousMonth()
        {
            var previousMonth = DateTime.Today.AddMonths(-1);
            var endDate = GetEndDateOfMonth(previousMonth);
            var taskCompletionSource = new TaskCompletionSource<MonthRange>();
            _calendarView.MonthChanged += taskCompletionSource.SetResult;
            
            InvokeCalendarViewMethod("OnPreviousMonthClick", new object[] {null, EventArgs.Empty});
            var range = await taskCompletionSource.Task;
            
            Assert.AreEqual(endDate, range.End);
        }

        [Test]
        public async Task ShouldGetStartDateOfMonthRangeWhenNavigateToNextMonth()
        {
            var nextMonth = DateTime.Today.AddMonths(1);
            var startDate = GetStartDateOfMonth(nextMonth);
            var taskCompletionSource = new TaskCompletionSource<MonthRange>();
            _calendarView.MonthChanged += taskCompletionSource.SetResult;
            
            InvokeCalendarViewMethod("OnNextMonthClick", new object[] {null, EventArgs.Empty});
            var range = await taskCompletionSource.Task;
            
            Assert.AreEqual(startDate, range.Start);
        }
        
        [Test]
        public async Task ShouldGetEndDateOfMonthRangeWhenNavigateToNextMonth()
        {
            var nextMonth = DateTime.Today.AddMonths(1);
            var endDate = GetEndDateOfMonth(nextMonth);
            var taskCompletionSource = new TaskCompletionSource<MonthRange>();
            _calendarView.MonthChanged += taskCompletionSource.SetResult;
            
            InvokeCalendarViewMethod("OnNextMonthClick", new object[] {null, EventArgs.Empty});
            var range = await taskCompletionSource.Task;
            
            Assert.AreEqual(endDate, range.End);
        }

        [Test]
        public async Task ShouldGetSelectedDayWhenSelectADay()
        {
            var firstDayOfCurrentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var taskCompletionSource = new TaskCompletionSource<DaySelected>();
            _calendarView.DaySelected += taskCompletionSource.SetResult;

            var firstValidCalendarDay = _calendarView
                .FindByName<Grid>("CalendarDaysContainer")
                .Children
                .Cast<CalendarDay>()
                .First(x => x.Day is {});
            var tap = (TapGestureRecognizer)firstValidCalendarDay.GestureRecognizers.First();
            tap.SendTapped(firstValidCalendarDay);
            var daySelected = await taskCompletionSource.Task;
            
            Assert.AreEqual(firstDayOfCurrentMonth, daySelected.DateTime);
        }
        
        [Test]
        public async Task ShouldGetEventsWhenSelectADay()
        {
            var firstDayOfCurrentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var calendarEvent = new Event(1, "Event name", firstDayOfCurrentMonth, firstDayOfCurrentMonth, false);
            _calendarView.Events = new List<Event> {calendarEvent};
            var taskCompletionSource = new TaskCompletionSource<DaySelected>();
            _calendarView.DaySelected += taskCompletionSource.SetResult;

            var firstValidCalendarDay = _calendarView
                .FindByName<Grid>("CalendarDaysContainer")
                .Children
                .Cast<CalendarDay>()
                .First(x => x.Day is {});
            var tap = (TapGestureRecognizer)firstValidCalendarDay.GestureRecognizers.First();
            tap.SendTapped(firstValidCalendarDay);
            var daySelected = await taskCompletionSource.Task;
            
            Assert.AreEqual(calendarEvent, daySelected.Events.First());
        }

        [Test]
        public void ShouldGetAddedEvents()
        {
            var events = new ObservableCollection<Event>();
            _calendarView.Events = events;
            var calendarEvent = new Event(1, "Event name", DateTime.Now, DateTime.Now, false);

            events.Add(calendarEvent);
            
            Assert.AreEqual(calendarEvent, _calendarView.Events.First());
        }

        [Test]
        public void ShouldDiscardRemovedEvents()
        {
            var events = new ObservableCollection<Event>();
            _calendarView.Events = events;
            var calendarEvent = new Event(1, "Event name", DateTime.Now, DateTime.Now, false);
            events.Add(calendarEvent);

            events.Remove(calendarEvent);
            
            Assert.IsEmpty(_calendarView.Events);
        }

        [Test]
        public void ShouldDiscardAllEvents()
        {
            var events = new ObservableCollection<Event>();
            _calendarView.Events = events;
            var calendarEvent = new Event(1, "Event name", DateTime.Now, DateTime.Now, false);
            events.Add(calendarEvent);
            
            events.Clear();
            
            Assert.IsEmpty(_calendarView.Events);
        }

        [Test]
        public void ShouldReplaceEvent()
        {
            var events = new ObservableCollection<Event>();
            _calendarView.Events = events;
            var oldEvent = new Event(1, "Old event", DateTime.Now, DateTime.Now, false);
            var updatedEvent = new Event(1, "New event", DateTime.Now, DateTime.Now, false);
            events.Add(oldEvent);
            
            events[0] = updatedEvent;
            var replacedEvent = _calendarView.Events.First();
            
            Assert.AreEqual(updatedEvent, replacedEvent);
        }

        [Test]
        public void ShouldReplaceEventsCollection()
        {
            var oldEvents = new ObservableCollection<Event>();
            _calendarView.Events = oldEvents;
            
            _calendarView.Events = new ObservableCollection<Event>();
            var discartedEvent = new Event(1, "Event name", DateTime.Now, DateTime.Now, false);
            oldEvents.Add(discartedEvent);
            
            Assert.IsEmpty(_calendarView.Events);
        }
    }
}
