﻿using System;
using System.Collections.Generic;
using System.Linq;
using static Xalendar.Api.Extensions.MonthExtension;

namespace Xalendar.Api.Models
{
    public readonly struct Month : IEquatable<Month>
    {
        public DateTime MonthDateTime { get; }
        public IReadOnlyList<Day> Days { get; }

        public Month(DateTime dateTime)
        {
            MonthDateTime = dateTime;
            Days = GenerateDaysOfMonth(dateTime);
        }

        public bool Equals(Month other)
        {
            var yearValue = MonthDateTime.Year == other.MonthDateTime.Year;
            var monthValue = MonthDateTime.Month == other.MonthDateTime.Month;
            var days = Days.SequenceEqual(other.Days);

            return yearValue && monthValue && days;
        }

        public static bool operator ==(Month left, Month right) =>
            left.Equals(right);
        public static bool operator !=(Month left, Month right) =>
            !left.Equals(right);

        public override bool Equals(object obj) =>
            (obj is Month month) && (this.Equals(month));

        public override int GetHashCode()
        {
            var yearValue = MonthDateTime.Year;
            var monthValue = MonthDateTime.Month;
            var dayHash = GetListHasCode(Days);

            return (yearValue, monthValue, dayHash).GetHashCode();

            static int GetListHasCode(IReadOnlyList<Day> days)
            {
                var size = days.Count;
                unchecked
                {
                    var hash = 31;
                    for (var i = 0; i < size; i++)
                    {
                        var day = days[i];
                        hash = 17 * hash + day.GetHashCode();
                    }

                    return hash;
                }
            }
        }
    }
}
