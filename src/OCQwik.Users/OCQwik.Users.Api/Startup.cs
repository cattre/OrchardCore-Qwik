using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OCQwik.Users.Application.Dtos;
using OCQwik.Users.Infrastructure;
using OrchardCore.Apis;
using OrchardCore.Modules;
using OrchardCore.Users.Models;

namespace OCQwik.Users.Api;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddOCQwikUsersInfrastructure();
        services.AddObjectGraphType<User, UserQueryObjectType>();
    }

    public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
    }
}
