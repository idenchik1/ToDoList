using System.ComponentModel.DataAnnotations;

namespace ToDoListApi.Entities;

public class TaskCreate
{
    [Required]
    [MinLength(4)]
    [MaxLength(256)]
    public string TaskContent { get; set; }
}