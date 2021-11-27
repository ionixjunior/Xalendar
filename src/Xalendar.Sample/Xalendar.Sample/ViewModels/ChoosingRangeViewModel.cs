using System;

namespace Xalendar.Sample.ViewModels
{
    public class ChoosingRangeViewModel
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public ChoosingRangeViewModel()
        {
            StartDate = DateTime.Today.AddDays(-5);
            EndDate = DateTime.Today.AddDays(5);
        }
    }
}
