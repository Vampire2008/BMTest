using BMTest.Controllers;
using BMTest.Data;
using BMTest.Models;
using BMTest.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace BmTest.Tests
{
    [TestClass]
    public class TaskControllerTests
    {

        [TestMethod]
        public async Task CreateAsync()
        {
            var taskGuid = Guid.NewGuid();

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(ts => ts.CreateAsync()).Returns(Task.FromResult(taskGuid));

            var taskController = new TaskController(taskServiceMock.Object);

            var result = await taskController.CreateAsync();

            Assert.IsInstanceOfType(result, typeof(AcceptedResult));
            var acceptedResult = (AcceptedResult)result;
            Assert.AreEqual(acceptedResult.StatusCode, 202);
            Assert.AreEqual(acceptedResult.Value, taskGuid);
        }

        //private const string ExisitingTestTaskGuid =  
        [TestMethod]
        public async Task GetAsync_NotFound()
        {
            var taskServiceMock = new Mock<ITaskService>();

            //var taskGuid = Guid.Parse("{BD6A4209-AB13-4025-BD5F-C7945774233D}");
            taskServiceMock.Setup(ts => ts.GetAsync(It.IsAny<Guid>())).Returns(Task.FromResult<BmTask>(null));

            var taskController = new TaskController(taskServiceMock.Object);

            var result = await taskController.GetAsync(Guid.NewGuid());
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            var notFoundResult = (NotFoundResult)result;
            Assert.AreEqual(notFoundResult.StatusCode, 404);
        }


        [TestMethod]
        public async Task GetAsync_TaskResult()
        {
            var taskServiceMock = new Mock<ITaskService>();

            var taskGuid = Guid.NewGuid();
            var taskStatus = BmTaskStatus.Created;
            var taskUpdatedAt = new DateTime(2020, 9, 4, 13, 3, 3);

            taskServiceMock.Setup(ts => ts.GetAsync(taskGuid)).Returns(Task.FromResult(new BmTask
            {
                Id = taskGuid,
                Status = taskStatus,
                UpdatedAt = taskUpdatedAt
            }));

            var taskController = new TaskController(taskServiceMock.Object);

            var result = await taskController.GetAsync(taskGuid);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okObjectResult = (OkObjectResult)result;
            Assert.AreEqual(okObjectResult.StatusCode, 200);
            Assert.IsInstanceOfType(okObjectResult.Value, typeof(TaskResponseModel));
            var model = (TaskResponseModel)okObjectResult.Value;
            Assert.AreEqual(model.Status, taskStatus);
            Assert.AreEqual(model.Timestamp, taskUpdatedAt);
        }
    }
}
