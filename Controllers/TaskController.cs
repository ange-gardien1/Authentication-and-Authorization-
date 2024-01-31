using challegeToDo_Backend.Models;
using challegeToDo_Backend.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace challegeToDo_Backend.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ILogger<TaskController>_logger;
    private readonly ITaskRepository _TaskRepository;

    public TaskController(ILogger<TaskController> logger, ITaskRepository repository)
    {
        _logger = logger;
        _TaskRepository = repository;
    }
  [HttpPost]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public ActionResult<task> CreateTask(Task task)
  {
    if (!ModelState.IsValid || task == null)
    {
        return BadRequest();
    }
    if (HttpContext.User == null)
    {
        return Unauthorized();
    }

    var userId = HttpContext.User.Claims.FirstOrDefault (c => c.Type == "task_userID");
    task.userId = Int32.Parse(userId.Value);

    var newTask = _TaskRepository.CreateTask(task);
    return Created(nameof(GetTaskById), newTask);
  }

}