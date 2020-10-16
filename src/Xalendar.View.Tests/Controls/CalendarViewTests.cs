using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Xalendar.View.Controls;
using Xamarin.Forms;

namespace Xalendar.View.Tests.Controls
{
    [TestFixture]
    public class CalendarViewTests
    {
        private CalendarView _calendarView;
        
        [SetUp]
        public void SetUp()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
            _calendarView = new CalendarView();
        }

        [Test]
        public void ShouldChangeMonthNameWhenPreviousButtonClicked()
        {
            var button = _calendarView.FindByName<Button>("PreviousButton");
            var monthName = _calendarView.FindByName<Label>("MonthName");

            _calendarView.MonthChanged += _ =>
            {
                var previousMonth = DateTime.Today.AddMonths(-1);
                Assert.AreEqual(previousMonth.ToString("MMMM yyyy"), monthName.Text);
            };
            
            button.SendClicked();
        }

        [Test]
        public void ShouldChangeMonthNameWhenNextButtonClicked()
        {
            var button = _calendarView.FindByName<Button>("NextButton");
            var monthName = _calendarView.FindByName<Label>("MonthName");

            _calendarView.MonthChanged += _ =>
            {
                var nextMonth = DateTime.Today.AddMonths(1);
                Assert.AreEqual(nextMonth.ToString("MMMM yyyy"), monthName.Text);
            };
            
            button.SendClicked();
        }
    }
}
