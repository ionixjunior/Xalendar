using System;
using System.Globalization;
using NUnit.Framework;
using Xalendar.Api.Formatters;

namespace Xalendar.Tests.Api.Formatters
{
    [TestFixture]
    public class DayOfWeek1CaractersFormatTests
    {
        [Test]
        [TestCaseSource(nameof(ValuesForTheDaysOfWeekInSpecificLanguagesTests))]
        public void MonthContainerShouldContainsTheDaysOfWeekInSpecificLanguages(string language, DayOfWeek dayOfWeek, string dayOfWeekName)
        {
            CultureInfo.CurrentCulture = new CultureInfo(language);
            var formatter = new DayOfWeek1CaractersFormat();

            var result = formatter.Format(dayOfWeek);

            Assert.AreEqual(dayOfWeekName, result);
        }

        private static object[] ValuesForTheDaysOfWeekInSpecificLanguagesTests =
        {
            new object[] { "pt-BR", DayOfWeek.Sunday, "d" },
            new object[] { "en-US", DayOfWeek.Sunday, "s" },
            new object[] { "fr-FR", DayOfWeek.Sunday, "d" }
        };
    }
}
