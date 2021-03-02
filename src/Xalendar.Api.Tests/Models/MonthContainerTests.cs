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

            Assert.IsFalse(monthContainer._currentMonth.Days.Any(day => day.HasEvents));
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

            Assert.IsTrue(monthContainer._currentMonth.Days.Any(day => day.HasEvents));
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
            var nextMonth = new Month(dateTime.AddMonths(1));

            monthContainer.Next();

            Assert.AreEqual(nextMonth, monthContainer._currentMonth);
        }

        [Test]
        public void MonthContainerShouldNavigateToPreviousMonth()
        {
            var dateTime = new DateTime(2021, 1, 1);
            var monthContainer = new MonthContainer(dateTime);
            var previousMonth = new Month(dateTime.AddMonths(-1));

            monthContainer.Previous();

            Assert.AreEqual(previousMonth, monthContainer._currentMonth);
        }

        [Test]
        public void MonthContainerShouldNavigateToNextMonthWhenPreviewDaysIsActive()
        {
            var dateTime = new DateTime(2020, 11, 9);
            var monthContainer = new MonthContainer(dateTime, isPreviewDaysActive: true);
            var nextMonth = monthContainer._nextMonth;

            monthContainer.Next();

            Assert.AreEqual(nextMonth, monthContainer._currentMonth);
        }

        [Test]
        public void MonthContainerShouldNavigateToPreviousMonthWhenPreviewDaysIsActive()
        {
            var dateTime = new DateTime(2021, 1, 1);
            var monthContainer = new MonthContainer(dateTime, isPreviewDaysActive: true);
            var previousMonth = monthContainer._previousMonth;

            monthContainer.Previous();

            Assert.AreEqual(previousMonth, monthContainer._currentMonth);
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
            new object[] { "pt-BR", "dom" },
            new object[] { "en-US", "sun" },
            new object[] { "fr-FR", "dim" }
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
            new object[] { "en-US", DayOfWeek.Sunday, new List<string> { "sun", "mon", "tue", "wed", "thu", "fri", "sat" } },
            new object[] { "en-US", DayOfWeek.Monday, new List<string> { "mon", "tue", "wed", "thu", "fri", "sat", "sun" } },
            new object[] { "en-US", DayOfWeek.Tuesday, new List<string> { "tue", "wed", "thu", "fri", "sat", "sun", "mon" } },
            new object[] { "en-US", DayOfWeek.Wednesday, new List<string> { "wed", "thu", "fri", "sat", "sun", "mon", "tue" } },
            new object[] { "en-US", DayOfWeek.Thursday, new List<string> { "thu", "fri", "sat", "sun", "mon", "tue", "wed" } },
            new object[] { "en-US", DayOfWeek.Friday, new List<string> { "fri", "sat", "sun", "mon", "tue", "wed", "thu" } },
            new object[] { "en-US", DayOfWeek.Saturday, new List<string> { "sat", "sun", "mon", "tue", "wed", "thu", "fri" } }
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

            Assert.IsFalse(monthContainer._currentMonth.Days.Any(day => day.HasEvents));
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

            Assert.IsFalse(monthContainer._currentMonth.Days.Any(day => day.HasEvents));
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
        public void ShouldGetFirstDayOfMonthContainerWhenPreviewDaysIsActive()
        {
            var dateTime = new DateTime(2020, 10, 1);
            var monthContainer = new MonthContainer(dateTime, isPreviewDaysActive: true);

            var firstDay = monthContainer.FirstDay;

            Assert.AreEqual(new DateTime(2020, 9, 27, 0, 0, 0), firstDay);
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
        public void ShouldGetLastDayOfMonthContainerWhenPreviewDaysIsActive()
        {
            var dateTime = new DateTime(2020, 10, 1);
            var monthContainer = new MonthContainer(dateTime, isPreviewDaysActive: true);

            var lastDay = monthContainer.LastDay;

            Assert.AreEqual(new DateTime(2020, 11, 7, 23, 59, 59), lastDay);
        }

        [Test]
        [TestCaseSource(nameof(ValuesForDaysOfMonthShouldStartBasedOnFirstDayOfWeek))]
        public void DaysOfMonthShouldStartBasedOnFirstDayOfWeek(DateTime dateTime, DayOfWeek firstDayOfWeek, int index)
        {
            var monthContainer = new MonthContainer(dateTime, firstDayOfWeek);

            var days = monthContainer.Days;

            var firstDayOfCurrentMonth = days.First(day => day is { } && day.DateTime.Day == 1);
            Assert.AreEqual(firstDayOfCurrentMonth, days.ElementAt(index));
        }

        private static object[] ValuesForDaysOfMonthShouldStartBasedOnFirstDayOfWeek =
        {
            new object[] { new DateTime(2020, 9, 1), DayOfWeek.Sunday, 2},
            new object[] { new DateTime(2020, 9, 1), DayOfWeek.Monday, 1},
            new object[] { new DateTime(2020, 9, 1), DayOfWeek.Tuesday, 0},
            new object[] { new DateTime(2020, 9, 1), DayOfWeek.Wednesday, 6},
            new object[] { new DateTime(2020, 9, 1), DayOfWeek.Thursday, 5},
            new object[] { new DateTime(2020, 9, 1), DayOfWeek.Friday, 4},
            new object[] { new DateTime(2020, 9, 1), DayOfWeek.Saturday, 3}
        };

        [Test]
        public void DayOfWeekShouldUseACustomFormat()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            var dateTime = DateTime.Today;
            var dayOfWeekFormatter = new CustomDayOfWeekFormatter();
            var monthContainer = new MonthContainer(dateTime, dayOfWeekFormatter, firstDayOfWeek: DayOfWeek.Sunday);

            var firstDayOfWeek = monthContainer.DaysOfWeek.First();

            Assert.AreEqual("SUNDAY", firstDayOfWeek);
        }

        public class CustomDayOfWeekFormatter : IDayOfWeekFormatter
        {
            public string Format(DayOfWeek dayOfWeek)
            {
                return CultureInfo
                    .CurrentCulture
                    .DateTimeFormat
                    .GetDayName(dayOfWeek)
                    .ToUpper();
            }
        }
    }
}
