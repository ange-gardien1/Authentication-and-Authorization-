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
  public ActionResult<task> CreateTask(task task)
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
  [HttpGet]
  public ActionResult<IEnumerable<task>> GetAllTasks()
  {
    return Ok(_TaskRepository.GetTasks());
  }
  [HttpGet]
  [Route("{taskId:int}")]
  public ActionResult<task> GetTaskById(int taskId)
  {
    var tasks = _TaskRepository.GetTaskById(taskId);
    if (tasks == null)
    {
        return NotFound();
    }
    return Ok(tasks);
  }

  [HttpPut]
  [Route("{taskId:int}")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public ActionResult<task> updateTask(task task)
  {
    if(!ModelState.IsValid || task == null)
    {
        return BadRequest();
    }
    return Ok(_TaskRepository.UpdateTask(task));

  }
  [HttpDelete]
  [Route("{taskId:int}")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public ActionResult DeleteTask(int taskId)
  {
    _TaskRepository.DeleteTaskById(taskId);
    return NoContent();
  }
}