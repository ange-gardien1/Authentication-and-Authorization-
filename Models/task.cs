namespace challegeToDo_Backend.Models;

public class task
{
    public int? taskId {get; set;}
    public string? taskName {get; set;}
    public bool completed {get; set;}
    public int userId{get; set;}
    public user? user {get; set;}
    public DateTime CreatedOn { get; set; }

}