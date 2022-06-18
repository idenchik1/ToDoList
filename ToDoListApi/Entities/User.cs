using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ToDoListApi.Entities;

[Index("UserName", Name = "UniqueName", IsUnique = true)]
public class User
{
    public User()
    {
        Lists = new HashSet<List>();
    }

    [JsonIgnore] [Key] public int UserId { get; set; }
    [Required] [StringLength(32)] public string UserName { get; set; } = null!;
    [JsonIgnore] [StringLength(64)] public string UserPassword { get; set; } = null!;
    [JsonIgnore] [StringLength(16)] public string UserPasswordSalt { get; set; } = null!;
    [JsonIgnore] public bool UserIsAdmin { get; set; }

    [InverseProperty("ListOwnerNavigation")]
    public virtual ICollection<List> Lists { get; set; }
}