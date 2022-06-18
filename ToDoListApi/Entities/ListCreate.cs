using System.ComponentModel.DataAnnotations;

namespace ToDoListApi.Entities;

public class ListCreate
{
    [Required]
    [MinLength(4)]
    [MaxLength(64)]
    public string listName { get; set; }
}