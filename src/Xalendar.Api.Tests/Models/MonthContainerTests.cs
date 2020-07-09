using System;
using NUnit.Framework;
using Xalendar.Api.Models;

namespace Xalendar.Api.Tests.Models
{
    [TestFixture]
    public class MonthContainerTests
    {
        [Test]
        public void MonthContainerShouldContainsDaysOfMonth()
        {
            var dateTime = DateTime.Now;
            
            var monthContainer = new MonthContainer(dateTime);
            
            Assert.IsNotEmpty(monthContainer.Days);
        }

        [Test]
        public void MonthContainerCanSelectDay()
        {
            var dateTime = new DateTime(2020, 7, 9);
            var monthContainer = new MonthContainer(dateTime);
            var selectedDay = new Day(dateTime, true);

            monthContainer.SelectDay(selectedDay);
            
            Assert.AreEqual(selectedDay, monthContainer.GetSelectedDay());
        }
    }
}
