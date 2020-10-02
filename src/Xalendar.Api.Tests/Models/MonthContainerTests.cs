using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using Xalendar.Api.Extensions;
using Xalendar.Api.Interfaces;
using Xalendar.Api.Models;

namespace Xalendar.Api.Tests.Models
{
    [TestFixture]
    public class MonthContainerTests
    {
        [Test]
        [TestCaseSource(nameof(MonthsValuesTests))]
        public void MonthContainerShouldHave42Days(DateTime dateTime)
        {
            var monthContainer = new MonthContainer(dateTime);

            Assert.IsNotEmpty(monthContainer.Days);
            Assert.AreEqual(42, monthContainer.Days.Count);
        }

        private static object[] MonthsValuesTests =
        {
            new object[] { new DateTime(2020, 7, 23) },
            new object[] { new DateTime(2015, 2, 10) },
            new object[] { new DateTime(2020, 8, 1) }
        };

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
            var events = new List<ICalendarViewEvent>
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
            var dateTimeName = dateTime.ToString("MMMM yyyy");
            Assert.AreEqual(dateTimeName, result);
        }

        [Test]
        public void MonthContainerShouldNavigateToNextMonth()
        {
            var dateTime = new DateTime(2020, 11, 9);
            var monthContainer = new MonthContainer(dateTime);

            monthContainer.Next();

            var dateTimeName = monthContainer._month.MonthDateTime.ToString("MMMM yyyy");
            Assert.AreEqual(dateTimeName, monthContainer.GetName());
            Assert.AreEqual(31, monthContainer.Days.Count(day => day is {}));
        }

        [Test]
        public void MonthContainerShouldNavigateToPreviousMonth()
        {
            var dateTime = new DateTime(2021, 1, 1);
            var monthContainer = new MonthContainer(dateTime);

            monthContainer.Previous();

            var dateTimeName = monthContainer._month.MonthDateTime.ToString("MMMM yyyy");
            Assert.AreEqual(dateTimeName, monthContainer.GetName());
        }

        [Test]
        [TestCaseSource(nameof(ValuesForTheDaysOfWeekInSpecificLanguagesTests))]
        public void MonthContainerShouldContainsTheDaysOfWeekInSpecificLanguages(string language, string dayOfWeekName)
        {
            CultureInfo.CurrentCulture = new CultureInfo(language);
            var dateTime = DateTime.Today;
            var monthContainer = new MonthContainer(dateTime);
            
            Assert.AreEqual(dayOfWeekName, monthContainer.DaysOfWeek.First());
        }
        
        private static object[] ValuesForTheDaysOfWeekInSpecificLanguagesTests =
        {
            new object[] { "pt-BR", "DOM" },
            new object[] { "en-US", "SUN" },
            new object[] { "fr-FR", "DIM" }
        };

        [Test]
        public void EventsShouldBeRemovedFromMonthContainer()
        {
            var dateTime = new DateTime(2020, 7, 9);
            var monthContainer = new MonthContainer(dateTime);
            var calendarViewEvent = new Event(1, "Name event", dateTime, dateTime, false);
            var events = new List<ICalendarViewEvent> {calendarViewEvent};
            monthContainer.AddEvents(events);
            
            monthContainer.RemoveEvent(calendarViewEvent);

            Assert.IsFalse(monthContainer._month.Days.Any(day => day.HasEvents));
        }

        [Test]
        public void AllEventsShouldBeRemovedFromMonthContainer()
        {
            var dateTime = new DateTime(2020, 7, 9);
            var monthContainer = new MonthContainer(dateTime);
            var calendarViewEvent = new Event(1, "Name event", dateTime, dateTime, false);
            var events = new List<ICalendarViewEvent> {calendarViewEvent};
            monthContainer.AddEvents(events);
            
            monthContainer.RemoveAllEvents();

            Assert.IsFalse(monthContainer._month.Days.Any(day => day.HasEvents));
        }

        [Test]
        public void DaysAttributeShouldBePopulateOneTime()
        {
            var dateTime = new DateTime(2020, 10, 1);
            var monthContainer = new MonthContainer(dateTime);

            var days = monthContainer.Days;
            
            Assert.AreEqual(days.GetHashCode(), monthContainer.Days.GetHashCode());
        }

        [Test]
        public void ShouldBeGetFirstDayOfMonthContainer()
        {
            var dateTime = new DateTime(2020, 10, 1);
            var monthContainer = new MonthContainer(dateTime);

            var firstDay = monthContainer.FirstDay;

            Assert.AreEqual(new DateTime(2020, 10, 1, 0, 0, 0), firstDay);
        }
    }
}
