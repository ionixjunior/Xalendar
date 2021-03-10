using System;

namespace Xalendar.Api.Models
{
    internal struct DayOfWeekName
    {
        public DayOfWeek DayOfWeek { get; }
        public string Name { get; }

        public DayOfWeekName(DayOfWeek dayOfWeek, string name)
        {
            DayOfWeek = dayOfWeek;
            Name = name;
        }
    }
}
