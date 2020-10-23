using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Xalendar.Api.Interfaces;
using Xalendar.View.Controls;
using Xamarin.Forms;

namespace Xalendar.View.Tests.Controls
{
    [TestFixture]
    public class CalendarViewTests : BaseTests
    {
        private CalendarView _calendarView;
        
        [SetUp]
        public void SetUp()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            _calendarView = new CalendarView();
        }

        [Test]
        public async Task ShouldChangeMonthNameWhenPreviousButtonClicked()
        {
            var button = _calendarView.FindByName<Button>("PreviousButton");
            var monthName = _calendarView.FindByName<Label>("MonthName");
            var previousMonth = DateTime.Today.AddMonths(-1);
            var taskCompletionSource = CreateTaskCompletionSource<string>();
            _calendarView.MonthChanged += _ => taskCompletionSource.SetResult(monthName.Text);
            
            button.SendClicked();
            
            Assert.AreEqual(previousMonth.ToString("MMMM yyyy"), await taskCompletionSource.Task);
        }

        [Test]
        public async Task ShouldChangeMonthNameWhenNextButtonClicked()
        {
            var button = _calendarView.FindByName<Button>("NextButton");
            var monthName = _calendarView.FindByName<Label>("MonthName");
            var nextMonth = DateTime.Today.AddMonths(1);
            var taskCompletionSource = CreateTaskCompletionSource<string>();
            _calendarView.MonthChanged += _ => taskCompletionSource.SetResult(monthName.Text);
            
            button.SendClicked();

            Assert.AreEqual(nextMonth.ToString("MMMM yyyy"), await taskCompletionSource.Task);
        }

        [Test]
        public void ShouldBeCreateEventsInCalendarView()
        {
            var eventsBinding = new Binding {Source = GenerateEvents()};
            
            _calendarView.SetBinding(CalendarView.EventsProperty, eventsBinding);
            
            Assert.IsNotEmpty(_calendarView.Events);
        }
    }
}
