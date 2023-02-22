using System.Collections.Generic;
using System.Threading.Tasks;
using OCQwik.Users.Application.Dtos;

namespace OCQwik.Users.Application.Queries;

public interface IUserQueries
{
    Task<IEnumerable<UserDto>> GetUsersAsync();
}
