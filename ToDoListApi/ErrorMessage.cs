namespace ToDoListApi;

public class ErrorMessage
{
    public ErrorMessage(List<string> errors)
    {
        Errors = errors;
    }

    public string Message => "Errors have occurred";
    public List<string> Errors { get; }
}