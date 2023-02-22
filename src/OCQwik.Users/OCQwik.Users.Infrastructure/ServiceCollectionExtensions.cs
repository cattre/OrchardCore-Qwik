using Microsoft.Extensions.DependencyInjection;
using OCQwik.Users.Application.Queries;
using OCQwik.Users.Infrastructure.Queries;

namespace OCQwik.Users.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOCQwikUsersInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserQueries, UserQueries>();

        return services;
    }
}
