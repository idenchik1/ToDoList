using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ToDoListApi.Entities;

[Index("UserName", Name = "UniqueName", IsUnique = true)]
public class User
{
    public User()
    {
        Lists = new HashSet<List>();
    }

    [Key] public int UserId { get; set; }
    [StringLength(32)] public string UserName { get; set; } = null!;
    [StringLength(64)] public string UserPassword { get; set; } = null!;
    [StringLength(16)] public string UserPasswordSalt { get; set; } = null!;
    public bool UserIsAdmin { get; set; }

    [InverseProperty("ListOwnerNavigation")]
    public virtual ICollection<List> Lists { get; set; }
}