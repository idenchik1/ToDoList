using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ToDoListApi.Entities;
using ToDoListApi.Model;

namespace ToDoListApi.Controllers;

[Route("api/auth")]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly ToDoListContext _context;
    private readonly Regex _passwordCheck;
    private readonly Regex _usernameCheck;

    public AuthController(ToDoListContext context)
    {
        _context = context;
        _usernameCheck = new Regex("^[a-zA-Z0-9]{6,32}$", RegexOptions.Compiled);
        _passwordCheck = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,128}$",
            RegexOptions.Compiled);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<Token>> Login(UserAuth user)
    {
        var token = GenerateToken(user.username, user.password);
        if (token != null) return Ok(token);

        return Unauthorized(new ErrorMessage(new List<string> { "Wrong username or password" }));
    }

    [HttpPost("Register")]
    public async Task<ActionResult<Token>> Register(UserAuth user)
    {
        if (_usernameCheck.IsMatch(user.username) && _passwordCheck.IsMatch(user.password))
        {
            if (UserExists(user.username))
                return BadRequest(new ErrorMessage(new List<string> { "Username already taken" }));

            var salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(12));
            var hashedPassword = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(user.password + salt)));
            _context.Users.Add(new User
            {
                UserName = user.username,
                UserPassword = hashedPassword,
                UserPasswordSalt = salt
            });
            await _context.SaveChangesAsync();
            var token = GenerateToken(user.username, user.password);
            return Ok(token);
        }

        return BadRequest(new ErrorMessage(new List<string> { "Username or password is incorrect" }));
    }

    private Token? GenerateToken(string username, string password)
    {
        var user = GetIdentity(username, password);
        if (user == null) return null;
        var now = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            AuthOptions.ISSUER,
            AuthOptions.AUDIENCE,
            notBefore: now,
            claims: user.Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        var token = new Token(new JwtSecurityTokenHandler().WriteToken(jwt));
        return token;
    }

    private bool UserExists(string username)
    {
        return (_context.Users?.Any(e => e.UserName == username)).GetValueOrDefault();
    }

    private ClaimsIdentity? GetIdentity(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(user => user.UserName == username);
        if (user != null)
        {
            var hashedPassword = SHA256.HashData(Encoding.UTF8.GetBytes(password + user.UserPasswordSalt));
            if (Convert.ToHexString(hashedPassword) == user.UserPassword)
            {
                var claims = new List<Claim>
                {
                    new(ClaimsIdentity.DefaultNameClaimType, user.UserId.ToString()),
                    new(ClaimsIdentity.DefaultRoleClaimType, user.UserIsAdmin.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Token",
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
        }

        return null;
    }
}