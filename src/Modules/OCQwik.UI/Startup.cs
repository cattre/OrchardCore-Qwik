using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrchardCore.Modules;

namespace OCQwik.UI
{
    public class Startup : StartupBase
    {
        private readonly IHostEnvironment _env;

        public Startup(IHostEnvironment environment)
        {
            _env = environment;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSpaStaticFiles(spa => spa.RootPath = "ClientApp/dist");
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSpaStaticFiles();

            app.Map("/app", spaApp =>
            {
                spaApp.UseSpa(spa =>
                {
                    // spa.Options.SourcePath = Path.Join(_env.ContentRootPath, "ClientApp");

                    spa.Options.SourcePath = "ClientApp";

                    // if (_env.IsDevelopment())
                    // {
                    //     spa.UseProxyToSpaDevelopmentServer("http://localhost:5173/");
                    // }
                });
            });
        }
    }
}
