using System;
using NUnit.Framework;
using Xalendar.Api.Models;

namespace Xalendar.Api.Tests.Models
{
    [TestFixture]
    public class MonthContainerTests
    {
        [Test]
        public void MonthContainerShouldContainsDaysOfMonth()
        {
            var dateTime = DateTime.Now;
            
            var monthContainer = new MonthContainer(dateTime);
            
            Assert.IsNotEmpty(monthContainer.Days);
        }
    }
}
