using System;
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
        public void ShouldNavigateToPreviousMonthWhenPreviousButtonClick()
        {
            var button = _calendarView.FindByName<Button>("PreviousButton");
            var monthName = _calendarView.FindByName<Label>("MonthName");
            
            button.SendClicked();

            var previousMonth = DateTime.Today.AddMonths(-1);
            Assert.AreEqual(previousMonth.ToString("MMMM yyyy"), monthName.Text);
        }
    }
}
