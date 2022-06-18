using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ToDoListApi.Entities;

public class Task
{
    [Key] public long TaskId { get; set; }
    [StringLength(256)] public string TaskContent { get; set; } = null!;

    [JsonIgnore] public int TaskList { get; set; }

    public bool TaskStatus { get; set; }

    [JsonIgnore]
    [ForeignKey("TaskList")]
    [InverseProperty("Tasks")]
    public virtual List TaskListNavigation { get; set; } = null!;
}