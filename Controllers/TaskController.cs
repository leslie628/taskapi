using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Model;
using TaskManagerApi.services;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskService _service;
        private readonly IAIService _aiService;
        public TaskController(ITaskService service, IAIService aiService)
        {
            _service = service;
            _aiService = aiService;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_service.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItem task)
        {
            await _service.Create(task);
            return Ok(task);
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> CreateBulk(List<CreateTaskRequest> tasks)
        {
            await _service.CreateBulk(tasks);
            return Ok(new
            {
                message = "Tasks created successfully"
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(TaskItem task)
        {
            _service.Update(task);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }
        [HttpPost("suggest")]
        public async Task<IActionResult> GetTaskSuggestion(TaskRequest request)
        {
            var suggestion = await _aiService.GenerateTaskSuggestion(request.Description);
            return Ok(suggestion);
        }
    }
}
