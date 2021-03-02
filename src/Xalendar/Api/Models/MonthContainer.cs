using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Xalendar.Extensions;

namespace Xalendar.Api.Models
{
    public class MonthContainer
    {
        internal Month _previousMonth;
        internal Month _currentMonth;
        internal Month _nextMonth;

        private IReadOnlyList<Day?>? _days;
        public IReadOnlyList<Day?> Days => _days ??= GetDaysOfContainer();
        public IReadOnlyList<string> DaysOfWeek { get; }

        public DateTime FirstDay => Days.First(day => day is {})!.DateTime;

        public DateTime LastDay => Days.Last(day => day is {})!.DateTime.AddHours(23).AddMinutes(59).AddSeconds(59);

        private DayOfWeek _firstDayOfWeek;
        
        public MonthContainer(DateTime dateTime, DayOfWeek firstDayOfWeek = DayOfWeek.Sunday)
        {
            _previousMonth = new Month(dateTime.AddMonths(-1));
            _currentMonth = new Month(dateTime);
            _nextMonth = new Month(dateTime.AddMonths(1));
            _firstDayOfWeek = firstDayOfWeek;

            DaysOfWeek = GenerateDaysOfWeek(firstDayOfWeek)
                .Select(GetDayOfWeekAbbreviated)
                .ToList();
        }

        private IEnumerable<DayOfWeek> GenerateDaysOfWeek(DayOfWeek firstDayOfWeek)
        {
            var daysOfWeek = new List<DayOfWeek>();

            for (var index = (int)firstDayOfWeek; index <= 6; index++)
                daysOfWeek.Add((DayOfWeek)index);
            
            for (var index = 0; index < (int)firstDayOfWeek; index++)
                daysOfWeek.Add((DayOfWeek)index);

            return daysOfWeek;
        }

        private string GetDayOfWeekAbbreviated(DayOfWeek dayOfWeek)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedDayName(dayOfWeek)
                .Substring(0, 3).ToUpper();
        }

        private IReadOnlyList<Day?> GetDaysOfContainer()
        {
            var daysOfContainer = new List<Day?>();
            //this.GetDaysToDiscardAtStartOfMonth(daysOfContainer);
            FillDaysOfPreviousMonth(daysOfContainer);
            daysOfContainer.AddRange(_currentMonth.Days);
            //this.GetDaysToDiscardAtEndOfMonth(daysOfContainer);
            FillDaysOfNextMonth(daysOfContainer);
            return daysOfContainer;
        }

        private void FillDaysOfPreviousMonth(List<Day?> daysOfContainer)
        {
            var firstDay = _currentMonth.Days.First();
            var differenceOfDays = ((int)_firstDayOfWeek - (int)firstDay!.DateTime.DayOfWeek);
            var numberOfDaysToShow = differenceOfDays <= 0 ? Math.Abs(differenceOfDays) : 7 - differenceOfDays;

            var days = _previousMonth
                .Days
                .Skip(_previousMonth.Days.Count - numberOfDaysToShow)
                .Take(numberOfDaysToShow);

            foreach (var day in days)
                daysOfContainer.Add(day);
        }

        private void FillDaysOfNextMonth(List<Day?> daysOfContainer)
        {
            if (daysOfContainer.Count < 42)
            {
                var numberOfDaysToShow = 42 - daysOfContainer.Count;

                var days = _nextMonth
                    .Days
                    .Take(numberOfDaysToShow);

                foreach (var day in days)
                    daysOfContainer.Add(day);
            }
        }

        private void GetDaysToDiscardAtStartOfMonth(List<Day?> daysOfContainer)
        {
            var firstDay = _currentMonth.Days.First();
            var differenceOfDays = ((int)_firstDayOfWeek - (int) firstDay!.DateTime.DayOfWeek);
            var numberOfDaysToDiscard = differenceOfDays <= 0 ? Math.Abs(differenceOfDays) : 7 - differenceOfDays;  
            
            for (var index = 0; index < numberOfDaysToDiscard; index++)
                daysOfContainer.Add(default(Day));
        }
        
        private void GetDaysToDiscardAtEndOfMonth(List<Day?> daysOfContainer)
        {
            if (daysOfContainer.Count < 42)
                for (var index = daysOfContainer.Count; index < 42; index++)
                    daysOfContainer.Add(default(Day));
        }
        
        public void Next()
        {
            var nextDateTime = _currentMonth.MonthDateTime.AddMonths(1);
            _previousMonth = new Month(nextDateTime.AddMonths(-1));
            _currentMonth = new Month(nextDateTime);
            _nextMonth = new Month(nextDateTime.AddMonths(1));
            _days = null;
        }

        public void Previous()
        {
            var previousDateTime = _currentMonth.MonthDateTime.AddMonths(-1);
            _previousMonth = new Month(previousDateTime.AddMonths(-1));
            _currentMonth = new Month(previousDateTime);
            _nextMonth = new Month(previousDateTime.AddMonths(1));
            _days = null;
        }
    }
}
