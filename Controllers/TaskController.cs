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
        public TaskController(ITaskService service)
        {
            _service = service;
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
    }
}
