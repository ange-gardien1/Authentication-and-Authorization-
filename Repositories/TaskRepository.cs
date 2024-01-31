using challegeToDo_Backend.Migrations;
using challegeToDo_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace challegeToDo_Backend.Repositories;
public class TaskRepository : ITaskRepository
{
    private readonly TodoDbContext _context;
    public TaskRepository(TodoDbContext context)
    {
        _context = context;
    }

    public task CreateTask(task newTask)
    {
       newTask.CreatedOn = DateTime.Now;
       _context.tasks.Add(newTask);
       _context.SaveChanges();
       return newTask;
    }

    public void DeleteTaskById(int taskId)
    {
        var taskToDelete = _context.tasks.FirstOrDefault(t => t.taskId == taskId);
        if (taskToDelete != null)
        {
            _context.tasks.Remove(taskToDelete);
            _context.SaveChanges();
        }
    }

    public task? GetTaskById(int taskId)
    {
      return _context.tasks.FirstOrDefault(t => t.taskId == taskId);
    }

    public IEnumerable<task> GetTasks()
    {
       return _context.tasks.ToList();
    }

    public IEnumerable<task> GetTasksByUserId(int userId)
    {
       return _context.tasks.Include(t => t.user).Where(c => c.userId == userId);
    }

    public task? UpdateTask(task updatedTask)
    {
        var existingTask = _context.tasks.FirstOrDefault(t => t.taskId == updatedTask.taskId);
        if(existingTask != null)
        {
            existingTask.taskName =updatedTask.taskName;
            existingTask.completed = updatedTask.completed;
            _context.SaveChanges();
        }
        return existingTask;
    }
}