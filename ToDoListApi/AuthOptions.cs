using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ToDoListApi;

public class AuthOptions
{
    public const string ISSUER = "ToDoServer"; // издатель токена
    public const string AUDIENCE = "ToDoClient"; // потребитель токена

    private const string KEY =
        "r3NabZQ$9pc#2$#twUR5aBeuK*hX^59!!gt9VP5n2f%f!xqT^dpDSWyjXod4zNSJzELYDzi*7KybtvL%@zjpn3AXo^wABm7J!ukR6K$$%mU*95LUm!4$BHNnDks3eH2b"; // ключ для шифрации

    public const int LIFETIME = 1440;

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}