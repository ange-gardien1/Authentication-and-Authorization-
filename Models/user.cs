using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace challegeToDo_Backend.Models;

public class user
{
    [JsonIgnore]
    public int userId{get; set;}
    [Required]
    public string? names {get; set;}
    [Required]
    [EmailAddress]
    public string? email {get; set;}
    [Required]
    public string? password {get; set;}
    [JsonIgnore]
    public IEnumerable<task>? tasks{get; set;}
}