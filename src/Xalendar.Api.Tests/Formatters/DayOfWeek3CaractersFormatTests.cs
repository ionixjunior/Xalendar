using System;
using System.Globalization;
using NUnit.Framework;
using Xalendar.Api.Formatters;

namespace Xalendar.Api.Tests.Formatters
{
    [TestFixture]
    public class DayOfWeek3CaractersFormatTests
    {
        [Test]
        [TestCaseSource(nameof(ValuesForTheDaysOfWeekInSpecificLanguagesTests))]
        public void MonthContainerShouldContainsTheDaysOfWeekInSpecificLanguages(string language, DayOfWeek dayOfWeek, string dayOfWeekName)
        {
            CultureInfo.CurrentCulture = new CultureInfo(language);
            var formatter = new DayOfWeek3CaractersFormat();

            var result = formatter.Format(dayOfWeek);

            Assert.AreEqual(dayOfWeekName, result);
        }

        private static object[] ValuesForTheDaysOfWeekInSpecificLanguagesTests =
        {
            new object[] { "pt-BR", DayOfWeek.Sunday, "dom" },
            new object[] { "en-US", DayOfWeek.Sunday, "sun" },
            new object[] { "fr-FR", DayOfWeek.Sunday, "dim" }
        };
    }
}
