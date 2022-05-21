using System.ComponentModel.DataAnnotations;

namespace ToDoListApi.Entities;

public class UserAuth
{
    [MinLength(6, ErrorMessage = "Username min length is 6")]
    [MaxLength(32, ErrorMessage = "Username max length is 32")]
    [Required(ErrorMessage = "Username is required")]
    public string username { get; set; }

    [MinLength(6, ErrorMessage = "Password min length is 8")]
    [MaxLength(128, ErrorMessage = "Password max length is 128")]
    [Required(ErrorMessage = "Password is required")]
    public string password { get; set; }
}