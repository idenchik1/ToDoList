using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListApi.Entities;
using ToDoListApi.Model;
using Task = ToDoListApi.Entities.Task;

namespace ToDoListApi.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly ToDoListContext _context;

    public UserController(ToDoListContext context)
    {
        _context = context;
    }

    private User GetUser(int id)
    {
        return _context.Users.Include(user => user.Lists).First(user => user.UserId == id);
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
        var userId = int.Parse(User.Identity.Name);
        if (_context.Users.Any(user => user.UserId == userId))
            return Ok();
        return Unauthorized();
    }

    [Authorize]
    [HttpGet("GetTasks/{listId:int}")]
    public async Task<ActionResult<ICollection<Task>>> GetTasks(int listId)
    {
        var userId = int.Parse(User.Identity.Name);
        var list = await _context.Lists.Include(selectedList => selectedList.Tasks)
            .FirstOrDefaultAsync(list => list.ListId == listId && list.ListOwner == userId);
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
            TaskContent = task.TaskContent,
            TaskList = listId
        });
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpDelete("DeleteTask/{taskId:int}")]
    public async Task<ActionResult> DeleteTask(int taskId)
    {
        var userId = int.Parse(User.Identity.Name);
        var task = await _context.Tasks.Include(task => task.TaskListNavigation)
            .FirstOrDefaultAsync(selectedList => selectedList.TaskId == taskId);
        if (task == null || task.TaskListNavigation.ListOwner != userId)
            return BadRequest(new ErrorMessage(new List<string> { "No task" }));
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpDelete("DeleteList/{listId:int}")]
    public async Task<ActionResult> DeleteList(int listId)
    {
        var userId = int.Parse(User.Identity.Name);
        var list = await _context.Lists.FirstOrDefaultAsync(selectedList => selectedList.ListId == listId);
        if (list == null || list.ListOwner != userId)
            return BadRequest(new ErrorMessage(new List<string> { "No list" }));
        _context.Lists.Remove(list);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpPut("EditTask/{taskId:int}")]
    public async Task<ActionResult> EditTask(TaskCreate newTask, int taskId)
    {
        var userId = int.Parse(User.Identity.Name);
        var task = await _context.Tasks.Include(task => task.TaskListNavigation)
            .FirstOrDefaultAsync(selectedList => selectedList.TaskId == taskId);
        if (task == null || task.TaskListNavigation.ListOwner != userId)
            return BadRequest(new ErrorMessage(new List<string> { "No task" }));
        task.TaskContent = newTask.TaskContent;
        await _context.SaveChangesAsync();
        return Ok();
    }
}