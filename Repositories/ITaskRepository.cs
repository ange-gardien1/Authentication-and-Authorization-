using challegeToDo_Backend.Models;

namespace challegeToDo_Backend.Repositories;

public interface ITaskRepository
{
    IEnumerable<task> GetTasks();
        task? GetTaskById(int taskId);
        IEnumerable<task> GetTasksByUserId(int userId);
        task CreateTask(task newTask);
        task? UpdateTask(task updatedTask);
        void DeleteTaskById(int taskId);
}
