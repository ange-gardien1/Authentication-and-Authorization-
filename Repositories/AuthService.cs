using bcrypt = BCrypt.Net.BCrypt;
using challegeToDo_Backend.Migrations;
using challegeToDo_Backend.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace challegeToDo_Backend.Repositories;

public class AuthService : IAuthService
{
    private static TodoDbContext _context;
    private static IConfiguration _config;

    public AuthService (TodoDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public user CreateUser(user user)
    {
       var passwordHash = bcrypt.HashPassword(user.password);
       user.password = passwordHash;

       _context.Add(user);
       _context.SaveChanges();
       return user;
    }

    public user? GetUserById(int id)
    {
          return _context.users.FirstOrDefault(u => u.userId == id);
    }

    public string Login(string email, string password)
    {
       var user = _context.users.SingleOrDefault(x => x.email == email);
       var verified = false;

       if (user != null)
       {
     verified = bcrypt.Verify(password, user.password);
       }

       if (user == null || !verified)
       {
        return string.Empty;
       }
       return BuildToken(user);

       
    }

    private string BuildToken(user user)
    {
        var secret = _config.GetValue<string>("TokenSecret");
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var signingCredentials = new SigningCredentials(signingKey,SecurityAlgorithms.HmacSha256);

        var claims = new Claim[]
        {
             new Claim(JwtRegisteredClaimNames.Sub, user.userId.ToString()),
            new Claim("task_userID", user.userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.names ?? ""),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.email ?? "")
          
        };

        var Jwt = new JwtSecurityToken(
            claims: claims,
            expires : DateTime.Now.AddMinutes(5),
            signingCredentials: signingCredentials
        );
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(Jwt);
        
        return encodedJwt;
    }
}