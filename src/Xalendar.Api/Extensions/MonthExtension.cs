using System.Linq;
using Xalendar.Api.Models;

namespace Xalendar.Api.Extensions
{
    public static class MonthExtension
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
    }
}