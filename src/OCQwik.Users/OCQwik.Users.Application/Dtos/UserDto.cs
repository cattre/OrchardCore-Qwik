using System.ComponentModel.DataAnnotations;
using GraphQL.Types;
using OrchardCore.Entities;
using OrchardCore.Users.Models;

namespace OCQwik.Users.Application.Dtos;

public record UserDto
{
    public UserDto (User user)
    {
        UserId = user.UserId;
        UserName = user.UserName;
        Email = user.Email;
    }

    [Required]
    public string UserId { get; init; }

    [Required]
    public string UserName { get; init; }

    [Required]
    public string Email { get; init; }
}

public class UserQueryObjectType : ObjectGraphType<User>
{
    public UserQueryObjectType()
    {
        Name = "User";

        // Map the fields you want to expose
        Field(x => x.UserId);
        Field(x => x.UserName);
        Field(x => x.Email);
    }
}
