using System;
using System.Threading;
using System.Threading.Tasks;

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
    }
}
