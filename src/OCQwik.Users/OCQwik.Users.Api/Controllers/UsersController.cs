using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OCQwik.Users.Application.Dtos;
using OCQwik.Users.Application.Queries;

namespace OCQwik.Users.Api.Controllers;

[ApiController,
 Route("api/users"),
 ApiExplorerSettings(GroupName = "OCQwik"),
 IgnoreAntiforgeryToken]
public class UsersController : ControllerBase
{
    private readonly IUserQueries _userQueries;

    public UsersController(IUserQueries userQueries)
    {
        _userQueries = userQueries;
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserDto>>> ListUsers()
    {
        return Ok(await _userQueries.GetUsersAsync());
    }
}
