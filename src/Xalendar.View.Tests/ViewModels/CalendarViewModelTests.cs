using NUnit.Framework;
using Xalendar.View.ViewModels;

namespace Xalendar.View.Tests.ViewModels
{
    [TestFixture]
    public class CalendarViewModelTests
    {
        [Test]
        public void ConstructorShouldBePopulateDaysAndDaysOfWeek()
        {
            var viewModel = new CalendarViewModel();
            
            Assert.NotNull(viewModel.Days);
            Assert.NotNull(viewModel.DaysOfWeek);
        }
    }
}
