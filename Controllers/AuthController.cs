using challegeToDo_Backend.Models;
using challegeToDo_Backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace challegeToDo_Backend.Controllers;
[ApiController]
[Route("api/[controller]")]
public class usersController : ControllerBase
{
   
    private readonly ILogger<usersController> _logger;
    private readonly IAuthService _authService;

    public usersController(ILogger<usersController> logger, IAuthService service)
    {
        _logger = logger;
        _authService = service;
    }
    [HttpPost]
    [Route("register")]
    public ActionResult CreateUser(user user)
    {
        if(user == null || !ModelState.IsValid)
        {
            return BadRequest("Invalid User Data");
        }
        if (_authService.IsEmailInUse(user.email))
        {
            return BadRequest("Email is arlead in use");
        }
        if(user.password.Length < 6 || !user.password.Any(char.IsDigit))
        {
            return BadRequest("Password must be at least 6 characters long and contain at least one digit");

        }
        _authService.CreateUser(user);
        return Ok( new {message = "Registration Successfully"});
    }
    [HttpGet]
    [Route("login")]
    public ActionResult<String> login(string email, string password)
    {
        if(string.IsNullOrWhiteSpace(email)|| string.IsNullOrWhiteSpace(password))
        {
            return BadRequest();
        }
        var token = _authService.Login(email, password);
        if (string.IsNullOrWhiteSpace(token))
        {
            return Unauthorized();
        }
        return Ok(token);
    }

    
    [HttpGet]
    [Route("current")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult<user> GetCurrentUser() 
    {

          var userEmail = User.Identity.Name;

          if(string.IsNullOrEmpty(userEmail))
          {
            return BadRequest("Invalid user Email");
          }

          

        if(!HttpContext.User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }
        if (HttpContext.User == null) {
            return Unauthorized();
        }
        
        var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Task_userID");

          if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
    {
        return Unauthorized();
    }
        var userId = Int32.Parse(userIdClaim.Value);

    if (!int.TryParse(userIdClaim.Value, out int UserId))
    {
        // Handle the case where parsing fails
        return BadRequest("Invalid user ID format");
    }

        var user = _authService.GetUserById(userId);

        if (user == null) {
             return NotFound("User not found");
        }

        return Ok(user);
    }
}