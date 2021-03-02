using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using Xalendar.Extensions;
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
        [TestCaseSource(nameof(ValuesForDaysOfWeekShouldStartWithSpecificDay))]
        public void DaysOfWeekShouldStartWithSpecificDay(string language, DayOfWeek firstDayOfWeek, List<string> expectedDaysOfWeek)
        {
            CultureInfo.CurrentCulture = new CultureInfo(language);
            var dateTime = DateTime.Today;
            var monthContainer = new MonthContainer(dateTime, firstDayOfWeek);

            var daysOfWeek = monthContainer.DaysOfWeek;

            CollectionAssert.AreEqual(expectedDaysOfWeek, daysOfWeek.ToList());
        }

        private static object[] ValuesForDaysOfWeekShouldStartWithSpecificDay =
        {
            new object[] { "en-US", DayOfWeek.Sunday, new List<string> { "SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT" } },
            new object[] { "en-US", DayOfWeek.Monday, new List<string> { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" } },
            new object[] { "en-US", DayOfWeek.Tuesday, new List<string> { "TUE", "WED", "THU", "FRI", "SAT", "SUN", "MON" } },
            new object[] { "en-US", DayOfWeek.Wednesday, new List<string> { "WED", "THU", "FRI", "SAT", "SUN", "MON", "TUE" } },
            new object[] { "en-US", DayOfWeek.Thursday, new List<string> { "THU", "FRI", "SAT", "SUN", "MON", "TUE", "WED" } },
            new object[] { "en-US", DayOfWeek.Friday, new List<string> { "FRI", "SAT", "SUN", "MON", "TUE", "WED", "THU" } },
            new object[] { "en-US", DayOfWeek.Saturday, new List<string> { "SAT", "SUN", "MON", "TUE", "WED", "THU", "FRI" } }
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
        public void ShouldGetFirstDayOfMonthContainer()
        {
            var dateTime = new DateTime(2020, 10, 1);
            var monthContainer = new MonthContainer(dateTime);

            var firstDay = monthContainer.FirstDay;

            Assert.AreEqual(new DateTime(2020, 10, 1, 0, 0, 0), firstDay);
        }

        [Test]
        public void ShouldGetLastDayOfMonthContainer()
        {
            var dateTime = new DateTime(2020, 10, 1);
            var monthContainer = new MonthContainer(dateTime);

            var lastDay = monthContainer.LastDay;
            
            Assert.AreEqual(new DateTime(2020, 10, 31, 23, 59, 59), lastDay);
        }

        [Test]
        [TestCaseSource(nameof(ValuesForDaysOfMonthShouldStartBasedOnFirstDayOfWeek))]
        public void DaysOfMonthShouldStartBasedOnFirstDayOfWeek(DateTime dateTime, DayOfWeek firstDayOfWeek, List<Day?> expectedDays)
        {
            var monthContainer = new MonthContainer(dateTime, firstDayOfWeek);

            var days = monthContainer.Days;
            
            CollectionAssert.AreEqual(expectedDays, days);
        }

        private static object[] ValuesForDaysOfMonthShouldStartBasedOnFirstDayOfWeek =
        {
            new object[] { new DateTime(2020, 9, 1), DayOfWeek.Sunday, GenerateDaysOfSeptember2020(2, 3) },
            new object[] { new DateTime(2020, 9, 1), DayOfWeek.Monday, GenerateDaysOfSeptember2020(1, 4) },
            new object[] { new DateTime(2020, 9, 1), DayOfWeek.Tuesday, GenerateDaysOfSeptember2020(0, 5) },
            new object[] { new DateTime(2020, 9, 1), DayOfWeek.Wednesday, GenerateDaysOfSeptember2020(6, 6) },
            new object[] { new DateTime(2020, 9, 1), DayOfWeek.Thursday, GenerateDaysOfSeptember2020(5, 7) },
            new object[] { new DateTime(2020, 9, 1), DayOfWeek.Friday, GenerateDaysOfSeptember2020(4, 1) },
            new object[] { new DateTime(2020, 9, 1), DayOfWeek.Saturday, GenerateDaysOfSeptember2020(3, 2) }
        };

        private static List<Day?> GenerateDaysOfSeptember2020(int daysToDiscardAtStart, int daysToDiscardAtEnd)
        {
            var days = new List<Day?>();
            
            for (var index = 0; index < daysToDiscardAtStart; index++)
                days.Add(default(Day));

            var dateTime = new DateTime(2020, 9, 1);
            for (var index = 1; index <= 30; index++)
            {
                days.Add(new Day(dateTime));
                dateTime = dateTime.AddDays(1);
            }
            
            for (var index = 0; index < daysToDiscardAtEnd; index++)
                days.Add(default(Day));
            
            if (days.Count < 42)
                for (var index = days.Count; index < 42; index++)
                    days.Add(default(Day));
            
            return days;
        }
    }
}
