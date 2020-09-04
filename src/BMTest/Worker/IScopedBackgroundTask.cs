using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZoomService.Worker;

namespace BMTest.Worker
{
    public interface IScopedBackgroundTask : IBackgroundTask
    {
        public Task DoWork(IServiceScope scope, CancellationToken token);

        public new Task DoWork(CancellationToken token)
        {
            throw new InvalidOperationException("Use method DoWork that accept a IServiceScope.");
        }
    }
}
