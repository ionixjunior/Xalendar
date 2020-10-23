using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xalendar.Api.Interfaces;

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
        
        protected List<ICalendarViewEvent> GenerateEvents()
        {
            return new List<ICalendarViewEvent>
            {
                new MyEvent(1, "Event name", DateTime.Now, DateTime.Now, false),
                new MyEvent(2, "Event name", DateTime.Now, DateTime.Now, false),
                new MyEvent(3, "Event name", DateTime.Now, DateTime.Now, false)
            };
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
