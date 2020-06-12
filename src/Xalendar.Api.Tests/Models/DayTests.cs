using System;
using NUnit.Framework;
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
    }
}
