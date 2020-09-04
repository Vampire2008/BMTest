using BMTest.Data;
using BMTest.Services.Interfaces;
using BMTest.Worker;
using BMTest.Worker.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BMTest.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly TaskContext _taskContext;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;

        public TaskService(TaskContext taskContext, IBackgroundTaskQueue backgroundTaskQueue)
        {
            _taskContext = taskContext;
            _backgroundTaskQueue = backgroundTaskQueue;
        }

        public async Task<Guid> CreateAsync()
        {
            var task = new BmTask
            {
                Status = BmTaskStatus.Created,
                UpdatedAt = DateTime.Now
            };
            _taskContext.Tasks.Add(task);
            await _taskContext.SaveChangesAsync();
            _backgroundTaskQueue.QueueBackgroundWorkItem(new CompleteTask(task.Id));
            return task.Id;
        }

        public async Task<BmTask> GetAsync(Guid taskId)
        {
            var task = await _taskContext.Tasks.AsNoTracking().SingleOrDefaultAsync(t => t.Id == taskId);
            return task;
        }
    }
}
