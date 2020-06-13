﻿using System;

namespace Xalendar.Api.Models
{
    public class Day
    {
        public DateTime DateTime { get; }

        public Day(DateTime dateTime, bool isSelected = false)
        {
            DateTime = dateTime;
            _isSelected = isSelected;
        }

        public bool IsToday => DateTime.Now.Day == DateTime.Day;

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => _isSelected = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Day dayToCompare)
                return dayToCompare.DateTime.Date.Ticks == DateTime.Date.Ticks;

            return false;
        }

        public override int GetHashCode() =>
            (DateTime.Date.Ticks).GetHashCode();
    }
}
