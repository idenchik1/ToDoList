using System.ComponentModel.DataAnnotations;

namespace ToDoListApi.Entities;

public record Token
{
    public Token(string accessToken)
    {
        access_token = accessToken;
    }

    [Required] public string access_token { get; }
}