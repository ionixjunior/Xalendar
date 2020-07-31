using NUnit.Framework;
using Xalendar.View.ViewModels;

namespace Xalendar.View.Tests.ViewModels
{
    [TestFixture]
    public class CalendarViewModelTests
    {
        [Test]
        public void ConstructorShouldBePopulateDefaultProperties()
        {
            var viewModel = new CalendarViewModel();
            
            Assert.NotNull(viewModel.Days);
            Assert.NotNull(viewModel.DaysOfWeek);
            Assert.NotNull(viewModel.MonthName);
        }
    }
}
