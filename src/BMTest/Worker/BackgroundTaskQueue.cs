using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using ZoomService.Worker;

namespace BMTest.Worker
{
    #region snippet1
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundWorkItem(IBackgroundTask workItem);

        Task<IBackgroundTask> DequeueAsync(
            CancellationToken cancellationToken);
    }

    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private ConcurrentQueue<IBackgroundTask> _workItems =
            new ConcurrentQueue<IBackgroundTask>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        public void QueueBackgroundWorkItem(
            IBackgroundTask workItem)
        {
            if (workItem == null)
                throw new ArgumentNullException(nameof(workItem));

            _workItems.Enqueue(workItem);
            _signal.Release();
        }

        public async Task<IBackgroundTask> DequeueAsync(
            CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);

            return workItem;
        }
    }
    #endregion
}
