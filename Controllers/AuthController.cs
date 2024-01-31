using challegeToDo_Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

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
}