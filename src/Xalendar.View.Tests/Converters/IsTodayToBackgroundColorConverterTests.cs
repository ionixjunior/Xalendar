using System;
using NUnit.Framework;
using Xalendar.Api.Models;
using Xalendar.View.Converters;
using Xamarin.Forms;

namespace Xalendar.View.Tests.Converters
{
    [TestFixture]
    public class IsTodayToBackgroundColorConverterTests
    {
        [Test]
        public void BackgroundColorShouldBeTransparentIfDayIsNull()
        {
            var day = default(Day);
            var converter = new IsTodayToBackgroundColorConverter();

            var result = (Color)converter.Convert(day, null, null, null);
            
            Assert.AreEqual(Color.Transparent, result);
        }
        
        [Test]
        public void BackgroundColorShouldBeTransparentIfDayIsNotToday()
        {
            var day = new Day(DateTime.Now.AddDays(1));
            var converter = new IsTodayToBackgroundColorConverter();

            var result = (Color)converter.Convert(day, null, null, null);
            
            Assert.AreEqual(Color.Transparent, result);
        }
        
        [Test]
        public void BackgroundColorShouldBeRedIfDayIsToday()
        {
            var day = new Day(DateTime.Now);
            var converter = new IsTodayToBackgroundColorConverter();

            var result = (Color)converter.Convert(day, null, null, null);
            
            Assert.AreEqual(Color.Red, result);
        }
    }
}
