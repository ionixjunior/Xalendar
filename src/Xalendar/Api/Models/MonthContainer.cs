﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xalendar.Api.Formatters;
using Xalendar.Api.Interfaces;

namespace Xalendar.Api.Models
{
    internal class MonthContainer
    {
        internal Month? _previousMonth;
        internal Month _currentMonth;
        internal Month? _nextMonth;

        private IReadOnlyList<Day?>? _days;
        public IReadOnlyList<Day?> Days => _days ??= GetDaysOfContainer();
        public IReadOnlyList<DayOfWeekName> DaysOfWeek { get; }

        public DateTime FirstDay => Days.First(day => day is {})!.DateTime;

        public DateTime LastDay => Days.Last(day => day is {})!.DateTime.AddHours(23).AddMinutes(59).AddSeconds(59);

        private DayOfWeek _firstDayOfWeek;
        private bool _isPreviewDaysActive;
        private IDayOfWeekFormatter _dayOfWeekFormatter;
        private DateTime? _startDate;
        private DateTime? _endDate;

        public MonthContainer(DateTime dateTime, DayOfWeek firstDayOfWeek = DayOfWeek.Sunday, DateTime? startDate = null, DateTime? endDate = null, 
            bool isPreviewDaysActive = false) : this(dateTime, new DayOfWeek3CaractersFormat(), startDate, endDate, firstDayOfWeek, isPreviewDaysActive)
        {
        }

        public MonthContainer(DateTime dateTime, IDayOfWeekFormatter dayOfWeekFormatter, DateTime? startDate = null, DateTime? endDate = null, DayOfWeek firstDayOfWeek = DayOfWeek.Sunday,
            bool isPreviewDaysActive = false)
        {
            _startDate = startDate;
            _endDate = endDate;
            _currentMonth = new Month(dateTime, startDate: _startDate, endDate: _endDate);

            if (isPreviewDaysActive)
            {
                _previousMonth = new Month(dateTime.AddMonths(-1), startDate: _startDate, endDate: _endDate, isCurrentMonth: false);
                _nextMonth = new Month(dateTime.AddMonths(1), startDate: _startDate, endDate: _endDate, isCurrentMonth: false);
            }

            _dayOfWeekFormatter = dayOfWeekFormatter;
            _firstDayOfWeek = firstDayOfWeek;
            _isPreviewDaysActive = isPreviewDaysActive;

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

        private DayOfWeekName GetDayOfWeekAbbreviated(DayOfWeek dayOfWeek) => new DayOfWeekName(dayOfWeek, _dayOfWeekFormatter.Format(dayOfWeek));

        private IReadOnlyList<Day?> GetDaysOfContainer()
        {
            var daysOfContainer = new List<Day?>();

            FillDaysOfPreviousMonth(daysOfContainer);
            FillDaysOfCurrentMonth(daysOfContainer);
            FillDaysOfNextMonth(daysOfContainer);

            return daysOfContainer;
        }

        private void FillDaysOfPreviousMonth(List<Day?> daysOfContainer)
        {
            if (_isPreviewDaysActive)
            {
                FillPreviewDaysOfPreviousMonth(daysOfContainer);
                return;
            }

            DiscardDaysOfPreviousMonth(daysOfContainer);
        }

        private void FillPreviewDaysOfPreviousMonth(List<Day?> daysOfContainer)
        {
            if (_previousMonth is Month { } previousMonth)
            {
                var firstDay = _currentMonth.Days.First();
                var differenceOfDays = ((int)_firstDayOfWeek - (int)firstDay!.DateTime.DayOfWeek);
                var numberOfDaysToShow = differenceOfDays <= 0 ? Math.Abs(differenceOfDays) : 7 - differenceOfDays;

                var days = previousMonth
                    .Days
                    .Skip(previousMonth.Days.Count - numberOfDaysToShow)
                    .Take(numberOfDaysToShow);

                foreach (var day in days)
                    daysOfContainer.Add(day);
            }
        }

        private void DiscardDaysOfPreviousMonth(List<Day?> daysOfContainer)
        {
            var firstDay = _currentMonth.Days.First();
            var differenceOfDays = ((int)_firstDayOfWeek - (int) firstDay!.DateTime.DayOfWeek);
            var numberOfDaysToDiscard = differenceOfDays <= 0 ? Math.Abs(differenceOfDays) : 7 - differenceOfDays;  
            
            for (var index = 0; index < numberOfDaysToDiscard; index++)
                daysOfContainer.Add(default(Day));
        }

        private void FillDaysOfCurrentMonth(List<Day?> daysOfContainer) => daysOfContainer.AddRange(_currentMonth.Days);

        private void FillDaysOfNextMonth(List<Day?> daysOfContainer)
        {
            if (_isPreviewDaysActive)
            {
                FillPreviewDaysOfNextMonth(daysOfContainer);
                return;
            }

            DiscardDaysOfNextMonth(daysOfContainer);
        }

        private void FillPreviewDaysOfNextMonth(List<Day?> daysOfContainer)
        {
            if (_nextMonth is Month { } nextMonth)
            {
                if (daysOfContainer.Count < 42)
                {
                    var numberOfDaysToShow = 42 - daysOfContainer.Count;

                    var days = nextMonth
                        .Days
                        .Take(numberOfDaysToShow);

                    foreach (var day in days)
                        daysOfContainer.Add(day);
                }
            }
        }

        private void DiscardDaysOfNextMonth(List<Day?> daysOfContainer)
        {
            if (daysOfContainer.Count < 42)
                for (var index = daysOfContainer.Count; index < 42; index++)
                    daysOfContainer.Add(default(Day));
        }
        
        public void Next()
        {
            var nextDateTime = _currentMonth.MonthDateTime.AddMonths(1);
            _currentMonth = new Month(nextDateTime, startDate: _startDate, endDate: _endDate);
            _days = null;

            if (_isPreviewDaysActive)
            {
                _previousMonth = new Month(nextDateTime.AddMonths(-1), startDate: _startDate, endDate: _endDate, isCurrentMonth: false);
                _nextMonth = new Month(nextDateTime.AddMonths(1), startDate: _startDate, endDate: _endDate, isCurrentMonth: false);
            }
        }

        public void Previous()
        {
            var previousDateTime = _currentMonth.MonthDateTime.AddMonths(-1);
            _currentMonth = new Month(previousDateTime, startDate: _startDate, endDate: _endDate);
            _days = null;

            if (_isPreviewDaysActive)
            {
                _previousMonth = new Month(previousDateTime.AddMonths(-1), startDate: _startDate, endDate: _endDate, isCurrentMonth: false);
                _nextMonth = new Month(previousDateTime.AddMonths(1), startDate: _startDate, endDate: _endDate, isCurrentMonth: false);
            }
        }
    }
}
