using System;
using System.Collections.Generic;
using System.Linq;
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

        [Test]
        public void MonthContainerShouldNotContainsEvents()
        {
            var dateTime = new DateTime(2020, 7, 9);
            var monthContainer = new MonthContainer(dateTime);
            
            Assert.IsFalse(monthContainer._month.Days.Any(day => day.HasEvents));
        }
        
        [Test]
        public void MonthContainerShouldContainsEvents()
        {
            var dateTime = new DateTime(2020, 7, 9);
            var monthContainer = new MonthContainer(dateTime);
            var events = new List<Event>
            {
                new Event(1, "Name event", dateTime, dateTime, false)
            };
            
            monthContainer.AddEvents(events);
            
            Assert.IsTrue(monthContainer._month.Days.Any(day => day.HasEvents));
        }

        [Test]
        public void MonthContainerShouldHaveAName()
        {
            var dateTime = new DateTime(2020, 7, 9);
            var monthContainer = new MonthContainer(dateTime);

            var result = monthContainer.GetName();
            
            Assert.AreEqual("July", result);
        }
    }
}
