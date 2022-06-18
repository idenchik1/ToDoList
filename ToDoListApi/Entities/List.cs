using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ToDoListApi.Entities;

public class List
{
    public List()
    {
        Tasks = new HashSet<Task>();
    }

    [Key] public int ListId { get; set; }
    [StringLength(64)] public string ListName { get; set; } = null!;
    [JsonIgnore] public int ListOwner { get; set; }

    [JsonIgnore]
    [ForeignKey("ListOwner")]
    [InverseProperty("Lists")]
    public virtual User ListOwnerNavigation { get; set; } = null!;

    [InverseProperty("TaskListNavigation")]
    [JsonIgnore]
    public virtual ICollection<Task> Tasks { get; set; }
}