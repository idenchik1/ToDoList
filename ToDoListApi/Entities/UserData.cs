namespace ToDoListApi.Entities;

public class UserData
{
    public string username { get; set; }
    public virtual ICollection<List> lists { get; set; }
}