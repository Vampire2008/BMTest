using BMTest.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BMTest.Worker.Tasks
{
    public class CompleteTask : IScopedBackgroundTask
    {
        private readonly Guid _taskId;

        public CompleteTask(Guid taskId)
        {
            _taskId = taskId;
        }

        public async Task DoWork(IServiceScope scope, CancellationToken token)
        {
            var context = scope.ServiceProvider.GetService<TaskContext>();
            var task = await context.Tasks.FindAsync(_taskId);
            task.Status = BmTaskStatus.Running;
            task.UpdatedAt = DateTime.Now;
            await context.SaveChangesAsync();
            await Task.Delay(120_000);
            task.Status = BmTaskStatus.Finished;
            task.UpdatedAt = DateTime.Now;
            await context.SaveChangesAsync();
        }

        public Task DoWork(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
