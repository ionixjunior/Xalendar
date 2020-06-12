using System;
using System.Linq;
using NUnit.Framework;
using Xalendar.Api.Models;

namespace Xalendar.Api.Tests.Models
{
    [TestFixture]
    public class MonthTests
    {
        [Test]
        public void June2020ShouldContains30Days()
        {
            var dateTime = new DateTime(2020, 6, 4);
            var month = new Month(dateTime);

            var result = month.GetDaysOfMonth();

            Assert.AreEqual(30, result.Count);
        }

        [Test]
        public void LeapYearFebruaryShouldBe29Days()
        {
            var dateTime = new DateTime(2020, 2, 1);
            var month = new Month(dateTime);

            var result = month.GetDaysOfMonth();

            Assert.AreEqual(29, result.Count);
        }

        [Test]
        public void ShouldBeNotExistSelectedDay()
        {
            var dateTime = new DateTime(2020, 2, 1);
            var month = new Month(dateTime);

            var selectedDay = month.GetSelectedDay();
            
            Assert.IsNull(selectedDay);
        }

        [Test]
        public void SelectedDayShouldBeToday()
        {
            var dateTime = DateTime.Today;
            var month = new Month(dateTime);
            var isSelected = true;
            var day = new Day(dateTime, isSelected);
            month.SelectDay(day);

            var selectedDay = month.GetSelectedDay();
            
            Assert.AreEqual(day, selectedDay);
        }

        [Test]
        public void SelectedDayShouldBeChanged()
        {
            
        }
    }
}
