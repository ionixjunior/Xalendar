using System;
using NUnit.Framework;
using Xalendar.Api.Models;

namespace Xalendar.Tests.Api.Models
{
    [TestFixture]
    public class DayOfWeekNameTests
    {
        [Test]
        public void ShouldGetTheDayOfWeek()
        {
            var dayOfWeek = DayOfWeek.Sunday;
            var name = "name";
            var dayOfWeekName = new DayOfWeekName(dayOfWeek, name);

            var result = dayOfWeekName.DayOfWeek;

            Assert.AreEqual(dayOfWeek, result);
        }

        [Test]
        public void ShouldGetTheName()
        {
            var dayOfWeek = DayOfWeek.Sunday;
            var name = "name";
            var dayOfWeekName = new DayOfWeekName(dayOfWeek, name);

            var result = dayOfWeekName.Name;

            Assert.AreEqual(name, result);
        }
    }
}
