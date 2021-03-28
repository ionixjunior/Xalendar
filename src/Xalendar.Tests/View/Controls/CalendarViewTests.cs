using System;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using Xalendar.View.Controls;

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
            var onPreviousMonthClick =
                calendarViewType.GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic);
            _ = onPreviousMonthClick.Invoke(_calendarView, parameters);
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
    }
}
