using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xalendar.Api.Interfaces;
using Xalendar.View.Controls;
using Xamarin.Forms;

namespace Xalendar.View.Tests
{
    public abstract class BaseTests
    {
        protected TaskCompletionSource<TResult> CreateTaskCompletionSource<TResult>()
        {
            var taskCompletionSource = new TaskCompletionSource<TResult>();
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            cancellationTokenSource.Token.Register(() => taskCompletionSource.TrySetCanceled(),
                useSynchronizationContext: false);
            return taskCompletionSource;
        }
        
        protected List<ICalendarViewEvent> GenerateEvents(int dayOfTheEvent)
        {
            var today = DateTime.Today;
            
            return new List<ICalendarViewEvent>
            {
                new MyEvent(1, "Event name", new DateTime(today.Year, today.Month, dayOfTheEvent), new DateTime(today.Year, today.Month, 1), false)
            };
        }

        protected CalendarDay FindCalendarDayByNumber(CalendarView calendarView, string number)
        {
            var calendarDaysContainer = calendarView.FindByName<FlexLayout>("CalendarDaysContainer");
            
            return (CalendarDay) calendarDaysContainer.Children.First(calendarDay =>
            {
                if (calendarDay is {})
                {
                    if (calendarDay.FindByName<Label>("DayElement") is {} dayElement)
                    {
                        if (dayElement.Text == number)
                            return true;
                    }
                }

                return false;
            });
        }
    }

    internal class MyEvent : ICalendarViewEvent
    {
        public object Id { get; }
        public string Name { get; }
        public DateTime StartDateTime { get; }
        public DateTime EndDateTime { get; }
        public bool IsAllDay { get; }

        public MyEvent(object id, string name, DateTime startDateTime, DateTime endDateTime, bool isAllDay)
        {
            Id = id;
            Name = name;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            IsAllDay = isAllDay;
        }
    }
}
