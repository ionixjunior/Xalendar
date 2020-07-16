using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Xalendar.Api.Extensions;
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

            var result = month.Days;

            Assert.AreEqual(30, result.Count);
        }

        [Test]
        public void LeapYearFebruaryShouldBe29Days()
        {
            var dateTime = new DateTime(2020, 2, 1);
            var month = new Month(dateTime);

            var result = month.Days;

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
        public void SelectedDayShouldBeTheFirstDayOfMonth()
        {
            var dateTime = new DateTime(2020, 2, 1);
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
            var dateTime = new DateTime(2020, 2, 1);
            var month = new Month(dateTime);
            var isSelected = true;
            var day = new Day(dateTime, isSelected);
            month.SelectDay(day);
            var newDaySelected = new Day(dateTime.AddDays(10), isSelected);
            month.SelectDay(newDaySelected);

            var selectedDay = month.GetSelectedDay();

            Assert.AreEqual(newDaySelected, selectedDay);
        }

        [Test]
        public void SelectDayFromAnotherMonthShouldBeFail()
        {
            var dateTime = new DateTime(2020, 2, 1);
            var month = new Month(dateTime);
            var isSelected = true;
            var day = new Day(dateTime.AddMonths(1), isSelected);
            month.SelectDay(day);

            var selectedDay = month.GetSelectedDay();

            Assert.IsNull(selectedDay);
        }

        [Test]
        public void MonthsShouldBeEquals()
        {
            var month1 = new Month(new DateTime(2020, 1, 1));
            var month2 = new Month(new DateTime(2020, 1, 10));

            var result = month1.Equals(month2);
            var hashCodeResult = month1.GetHashCode() == month2.GetHashCode();

            Assert.IsTrue(result);
            Assert.IsTrue(hashCodeResult);
        }

        [Test]
        public void MonthsNotShouldBeEquals()
        {
            var month1 = new Month(new DateTime(2020, 1, 1));
            var month2 = new Month(new DateTime(2020, 2, 1));

            var result = month1.Equals(month2);
            var hashCodeResult = month1.GetHashCode() == month2.GetHashCode();

            Assert.IsFalse(result);
            Assert.IsFalse(hashCodeResult);
        }

        [Test]
        public void ObjectsShouldBeEquals()
        {
            object object1 = new Month(new DateTime(2020, 1, 1));
            object object2 = new Month(new DateTime(2020, 1, 10));

            var result = object1.Equals(object2);

            Assert.IsTrue(result);
        }

        [Test]
        public void ObjectsNotShouldBeEquals()
        {
            object object1 = new Month(new DateTime(2020, 1, 1));
            object object2 = new Month(new DateTime(2020, 2, 1));

            var result = object1.Equals(object2);

            Assert.IsFalse(result);
        }

        [Test]
        public void MonthEqualsOperatorShouldBeEquals()
        {
            var month1 = new Month(new DateTime(2020, 1, 1));
            var month2 = new Month(new DateTime(2020, 1, 10));

            var result = month1 == month2;

            Assert.IsTrue(result);
        }

        [Test]
        public void MonthNotEqualsOperatorNotShouldBeEquals()
        {
            var month1 = new Month(new DateTime(2020, 1, 1));
            var month2 = new Month(new DateTime(2020, 2, 1));

            var result = month1 != month2;

            Assert.IsTrue(result);
        }

        [Test]
        public void HashCodeShouldBeEquals()
        {
            var month1 = new Month(new DateTime(2020, 1, 1));
            var month2 = new Month(new DateTime(2020, 1, 10));
            var hashCodeMonth1 = month1.GetHashCode();
            var hashCodeMonth2 = month2.GetHashCode();

            var result = hashCodeMonth1 == hashCodeMonth2;

            Assert.IsTrue(result);
        }

        [Test]
        public void HashCodeNotShouldBeEquals()
        {
            var month1 = new Month(new DateTime(2020, 1, 1));
            var month2 = new Month(new DateTime(2020, 2, 1));
            var hashCodeMonth1 = month1.GetHashCode();
            var hashCodeMonth2 = month2.GetHashCode();

            var result = hashCodeMonth1 == hashCodeMonth2;

            Assert.IsFalse(result);
        }

        [Test]
        public void MonthNotShouldContainsEvents()
        {
            var month = new Month(new DateTime(2020, 1, 1));

            var eventsOfMonth = month.Days.Where(day => day.Events.Any());

            Assert.IsEmpty(eventsOfMonth);
        }

        [Test]
        public void MonthShouldContainsEvents()
        {
            var dateTime = new DateTime(2020, 1, 1);
            var month = new Month(dateTime);
            var events = new List<Event>
            {
                new Event(1, "Name", dateTime, dateTime, false)
            };
            month.AddEvents(events);

            var eventsOfMonth = month.Days.Where(day => day.Events.Any());

            Assert.IsNotEmpty(eventsOfMonth);
            Assert.AreEqual(1, eventsOfMonth.Count());
        }

        [Test]
        public void EventsFromAnotherMonthNotShouldBeAdded()
        {
            var dateTime = new DateTime(2020, 1, 1);
            var month = new Month(dateTime);
            var events = new List<Event>
            {
                new Event(1, "Name", dateTime, dateTime, false),
                new Event(2, "Name", dateTime.AddMonths(1), dateTime.AddMonths(1), false)
            };
            month.AddEvents(events);

            var eventsOfMonth = month.Days.Where(day => day.Events.Any());

            Assert.IsNotEmpty(eventsOfMonth);
            Assert.AreEqual(1, eventsOfMonth.Count());
        }
    }
}
