using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListApi.Entities;

public class List
{
    public List()
    {
        Tasks = new HashSet<Task>();
    }

    [Key] public int ListId { get; set; }
    [StringLength(64)] public string ListName { get; set; } = null!;
    public int ListOwner { get; set; }

    [ForeignKey("ListOwner")]
    [InverseProperty("Lists")]
    public virtual User ListOwnerNavigation { get; set; } = null!;

    [InverseProperty("TaskListNavigation")]
    public virtual ICollection<Task> Tasks { get; set; }
}