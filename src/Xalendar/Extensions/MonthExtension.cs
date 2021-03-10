using System;
using System.Collections.Generic;
using System.Linq;
using Xalendar.Api.Interfaces;
using Xalendar.Api.Models;

namespace Xalendar.Extensions
{
    internal static class MonthExtension
    {
        public static void SelectDay(this Month month, Day selectedDay)
        {
            UnSelectCurrentDay(month);
            SelectNewDay(selectedDay, month);
        }

        private static void SelectNewDay(Day selectedDay, Month month)
        {
            var newSelectedDay = month.Days.FirstOrDefault(day => day.DateTime.Equals(selectedDay.DateTime));

            if (newSelectedDay is null)
                return;

            newSelectedDay.IsSelected = true;
        }

        private static void UnSelectCurrentDay(Month month)
        {
            var selectedDay = month.GetSelectedDay();
            if (selectedDay is null)
                return;

            selectedDay.IsSelected = false;
        }
        
        public static Day GetSelectedDay(this Month month)
        {
            return month.Days.FirstOrDefault(day => day.IsSelected);
        }
        
        public static List<Day> GenerateDaysOfMonth(DateTime dateTime, bool isCurrentMonth = true)
        {
            return Enumerable
                .Range(1, DateTime.DaysInMonth(dateTime.Year, dateTime.Month))
                .Select(dayValue => new DateTime(dateTime.Year, dateTime.Month, dayValue))
                .Select(dateTime => new Day(dateTime, isCurrentMonth: isCurrentMonth))
                .ToList();
        }

        public static void AddEvents(this Month month, IEnumerable<ICalendarViewEvent> events)
        {
            foreach (var @event in events)
            {
                var eventDate = @event.StartDateTime.Date;
                month.Days.FirstOrDefault(day => day.DateTime.Date.Equals(eventDate))?.AddEvent(@event);
            }
        }

        public static void RemoveEvent(this Month month, ICalendarViewEvent calendarViewEvent)
        {
            var eventDate = calendarViewEvent.StartDateTime.Date;
            month.Days.FirstOrDefault(day => day.DateTime.Date.Equals(eventDate))?.RemoveEvent(calendarViewEvent);
        }

        public static void RemoveAllEvents(this Month month)
        {
            foreach (var day in month.Days)
                day.RemoveAllEvents();
        }
    }
}
