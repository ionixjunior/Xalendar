using System;
using System.Collections.Generic;
using Xalendar.Api.Extensions;
using Xalendar.Api.Models;

namespace Xalendar.View.ViewModels
{
    public class CalendarViewModel
    {
        private readonly MonthContainer _monthContainer;
        
        public IReadOnlyList<Day?> Days { get; }
        public IReadOnlyList<string> DaysOfWeek { get; }
        public string MonthName { get; }
        
        public CalendarViewModel()
        {
            _monthContainer = new MonthContainer(DateTime.Today);
            Days = _monthContainer.Days;
            DaysOfWeek = _monthContainer.DaysOfWeek;
            MonthName = _monthContainer.GetName();
        }
    }
}
