using BMTest.Data;
using BMTest.Services.Implementations;
using BMTest.Worker;
using BMTest.Worker.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using ZoomService.Worker;

namespace BmTest.Tests
{
    [TestClass]
    public class TaskServiceTests
    {
        private readonly DbContextOptions<TaskContext> _dbOptions;

        public TaskServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<TaskContext>()
                .UseInMemoryDatabase("BmTasks")
                .Options;
        }

        [TestCleanup]
        public void ClearDatabase()
        {
            new TaskContext(_dbOptions).Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task CreateTaskAsync()
        {
            var taskQueueMock = new Mock<IBackgroundTaskQueue>();
            IBackgroundTask task = null;
            taskQueueMock.Setup(tq => tq.QueueBackgroundWorkItem(It.IsNotNull<IBackgroundTask>()))
                .Callback<IBackgroundTask>(r => task = r);

            var testContext = new TaskContext(_dbOptions);

            var taskService = new TaskService(testContext, taskQueueMock.Object);
            await taskService.CreateAsync();

            var taskInDb = testContext.Tasks.SingleOrDefault();

            Assert.IsNotNull(task);
            Assert.IsInstanceOfType(task, typeof(CompleteTask));
            Assert.IsNotNull(taskInDb);
            //Assert.AreEqual(taskInDb.Id, task.)
        }

        [TestMethod]
        public async Task GetTaskAsync_NoResult()
        {
            var taskQueueMock = new Mock<IBackgroundTaskQueue>();
            var testContext = new TaskContext(_dbOptions);

            var taskService = new TaskService(testContext, taskQueueMock.Object);

            var task = await taskService.GetAsync(Guid.NewGuid());

            Assert.IsNull(task);
        }


        [TestMethod]
        public async Task GetTaskAsync_SameResultAsAddedToDb()
        {
            var taskQueueMock = new Mock<IBackgroundTaskQueue>();

            var taskGuid = Guid.NewGuid();
            var taskStatus = BmTaskStatus.Created;
            var taskUpdatedAt = new DateTime(2020, 9, 4, 13, 3, 3);

            using (var context = new TaskContext(_dbOptions))
            {
                var newTask = new BmTask
                {
                    Id = taskGuid,
                    Status = taskStatus,
                    UpdatedAt = taskUpdatedAt
                };
                context.Add(newTask);
                await context.SaveChangesAsync();
            }

            var testContext = new TaskContext(_dbOptions);

            var taskService = new TaskService(testContext, taskQueueMock.Object);

            var task = await taskService.GetAsync(taskGuid);

            Assert.IsNotNull(task);
            Assert.AreEqual(task.Id, taskGuid);
            Assert.AreEqual(task.Status, taskStatus);
            Assert.AreEqual(task.UpdatedAt, taskUpdatedAt);
        }
    }
}
