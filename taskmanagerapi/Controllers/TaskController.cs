using Microsoft.AspNetCore.Mvc;
using taskmanagerapi.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace taskmanagerapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskService taskService;

        public TaskController(TaskService taskService)
        {
            this.taskService = taskService;
        }
        [HttpGet]
        [Route("gettasks")]
        public async Task<IActionResult> GetTasks([FromQuery] Guid userId)
        {
            return Ok(await taskService.GetTasks(userId));
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateTask(Models.Task task, [FromQuery] Guid userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data not valid");
            }
            return Ok(await taskService.CreateTask(task,userId));
        }   
        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data not valid");
            }
            await taskService.DeleteTask(taskId);
            return Ok();
        }
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateTask(Models.Task task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data not valid");
            }
            
            return Ok(await taskService.UpdateTask(task));
        }
    }
}
