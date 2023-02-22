using System.ComponentModel.DataAnnotations;
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
