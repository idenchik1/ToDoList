using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListApi.Entities;
using ToDoListApi.Model;
using Task = ToDoListApi.Entities.Task;

namespace ToDoListApi.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ToDoListContext _context;

    public UserController(ToDoListContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("GetUserInfo")]
    public async Task<ActionResult<User>> GetUserInfo()
    {
        var userId = int.Parse(User.Identity.Name);
        return GetUser(userId);
    }

    [Authorize]
    [HttpGet("IsAuthed")]
    public async Task<ActionResult> GetIsAuthed()
    {
        return Ok();
    }

    [Authorize]
    [HttpGet("GetTasks/{id:int}")]
    public async Task<ActionResult<ICollection<Task>>> GetTasks(int id)
    {
        var userId = int.Parse(User.Identity.Name);
        var list = await _context.Lists.Include(selectedList => selectedList.Tasks)
            .FirstOrDefaultAsync(list => list.ListId == id && list.ListOwner == userId);
        if (list != null)
            return Ok(list.Tasks);
        return BadRequest(new ErrorMessage(new List<string> { "No list" }));
    }

    [Authorize]
    [HttpPost("ChangeTaskStatus/{id:int}")]
    public async Task<ActionResult> ChangeTaskStatus(int id)
    {
        var userId = int.Parse(User.Identity.Name);
        var task = await _context.Tasks.Include(selectedTask => selectedTask.TaskListNavigation.ListOwnerNavigation)
            .FirstOrDefaultAsync(selectedTask => selectedTask.TaskId == id);
        if (task == null || task.TaskListNavigation.ListOwnerNavigation.UserId != userId)
            return BadRequest(new ErrorMessage(new List<string> { "No task" }));

        task.TaskStatus = !task.TaskStatus;
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpPost("CreateList")]
    public async Task<ActionResult> CreateList(ListCreate list)
    {
        var userId = int.Parse(User.Identity.Name);
        _context.Lists.Add(new List
        {
            ListName = list.listName,
            ListOwner = userId
        });
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpPost("CreateTask/{listId:int}")]
    public async Task<ActionResult> CreateTask(TaskCreate task, int listId)
    {
        var userId = int.Parse(User.Identity.Name);
        var list = await _context.Lists.FirstOrDefaultAsync(selectedList => selectedList.ListId == listId);
        if (list == null || list.ListOwner != userId)
            return BadRequest(new ErrorMessage(new List<string> { "No list" }));
        _context.Tasks.Add(new Task
        {
            TaskContent = task.taskContent,
            TaskList = listId
        });
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpDelete("DeleteList/{}")]
    private User GetUser(int id)
    {
        return _context.Users.Include(user => user.Lists).First(user => user.UserId == id);
    }
}