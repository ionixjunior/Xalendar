using System;
using System.Collections.Generic;
using NUnit.Framework;
using Xalendar.Api.Extensions;
using Xalendar.Api.Models;

namespace Xalendar.Api.Tests.Models
{
    [TestFixture]
    public class DayTests
    {
        [Test]
        public void DayShoudBeToday()
        {
            var dateTime = DateTime.Today;
            var day = new Day(dateTime);

            var result = day.IsToday;

            Assert.IsTrue(result);
        }

        [Test]
        public void DayNotShoudBeToday()
        {
            var dateTime = DateTime.Today.AddDays(1);
            var day = new Day(dateTime);

            var result = day.IsToday;

            Assert.IsFalse(result);
        }

        [Test]
        public void DayIsUnSelected()
        {
            var dateTime = DateTime.Today;
            var day = new Day(dateTime);

            var result = day.IsSelected;
            
            Assert.IsFalse(result);
        }
        
        [Test]
        public void DayIsSelected()
        {
            var dateTime = DateTime.Today;
            var isSelected = true;
            var day = new Day(dateTime, isSelected);

            var result = day.IsSelected;
            
            Assert.IsTrue(result);
        }
        
        [Test]
        public void DayIsSelectedWhenChangeState()
        {
            var dateTime = DateTime.Today;
            var day = new Day(dateTime);
            day.IsSelected = true;

            var result = day.IsSelected;
            
            Assert.IsTrue(result);
        }

        [Test]
        public void ParameterOfEqualsComparisonShouldNotBeDayClass()
        {
            var day = new Day(DateTime.Now);

            var comparison = day.Equals(new DateTime());
            
            Assert.IsFalse(comparison);
        }

        [Test]
        public void DayShouldBeEquals()
        {
            var dayOne = new Day(DateTime.Now);
            var dayTwo = new Day(DateTime.Now);

            var comparison = dayOne.Equals(dayTwo);
            var hashCodeComparision = dayOne.GetHashCode().Equals(dayTwo.GetHashCode());
            Assert.IsTrue(comparison);
            Assert.IsTrue(hashCodeComparision);
        }
        
        [Test]
        public void DayShouldNotBeEquals()
        {
            var dayOne = new Day(DateTime.Now);
            var dayTwo = new Day(DateTime.Now.AddDays(1));

            var comparison = dayOne.Equals(dayTwo);

            Assert.IsFalse(comparison);
        }

        [Test]
        public void EventsShoudBeEmpty()
        {
            var day = new Day(DateTime.Now);

            var events = day.Events;
            
            Assert.IsEmpty(events);
        }

        [Test]
        public void EventsShouldBeAdded()
        {
            var day = new Day(DateTime.Now);
            var eventsOfDay = new List<Event>
            {
                new Event(1, "Name", DateTime.Now, DateTime.Now, false)
            };
            day.AddEvents(eventsOfDay);

            var events = day.Events;
            
            Assert.IsNotEmpty(events);
        }
    }
}
