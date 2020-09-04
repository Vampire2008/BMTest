using BMTest.Models;
using BMTest.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BMTest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync()
        {
            var taskId = await _taskService.CreateAsync();
            return Accepted(taskId);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var task = await _taskService.GetAsync(id);
            if (task == null)
                return NotFound();
            var model = new TaskResponseModel
            {
                Status = task.Status,
                Timestamp = task.UpdatedAt
            };
            return Ok(model);
        }
    }
}
