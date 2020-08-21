using System;
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
        public void DayNotShouldBeTodayWhenIsAnotherMonth()
        {
            var currentDateTime = new DateTime(2020, 8, 20);
            var nextMonth = new DateTime(2020, 9, 20);
            var day = new Day(nextMonth, currentDateTime);

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
            var @event = new Event(1, "Name", DateTime.Now, DateTime.Now, false);
            day.AddEvent(@event);

            var events = day.Events;
            
            Assert.IsNotEmpty(events);
        }

        [Test]
        public void DayShouldNotHaveEvents()
        {
            var day = new Day(DateTime.Now);

            var hasEvents = day.HasEvents;
            
            Assert.IsFalse(hasEvents);
        }
        
        [Test]
        public void DayShouldHaveEvents()
        {
            var day = new Day(DateTime.Now);
            var @event = new Event(1, "Name", DateTime.Now, DateTime.Now, false);
            day.AddEvent(@event);

            var hasEvents = day.HasEvents;
            
            Assert.IsTrue(hasEvents);
        }

        [Test]
        public void NumberOfDayShouldBeAppearInString()
        {
            var datetime = new DateTime(2020, 1, 10);
            var day = new Day(datetime);

            var result = day.ToString();
            
            Assert.AreEqual("10", result);
        }

        [Test]
        [TestCaseSource(nameof(DataWeekendTests))]
        public void IsWeekendPropertyShouldBeCorrectly(DateTime dateTime, bool expectedResult)
        {
            var day = new Day(dateTime);

            var isWeekend = day.IsWeekend;
            
            Assert.AreEqual(expectedResult, isWeekend);
        }

        private static object[] DataWeekendTests =
        {
            new object[] { new DateTime(2020, 6, 29), false },
            new object[] { new DateTime(2020, 6, 30), false },
            new object[] { new DateTime(2020, 7, 1), false },
            new object[] { new DateTime(2020, 7, 2), false },
            new object[] { new DateTime(2020, 7, 3), false },
            new object[] { new DateTime(2020, 7, 4), true },
            new object[] { new DateTime(2020, 7, 5), true }
        };
    }
}
