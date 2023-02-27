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
            services.AddSpaStaticFiles(spa => spa.RootPath = "../OCQwik.UI/ClientApp");
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            app.UseSpaStaticFiles();
            app.Map("/app", spaApp =>
            {
                spaApp.UseSpa(spa =>
                {
                    spa.Options.SourcePath = Path.Join(_env.ContentRootPath, "../OCQwik.UI/ClientApp");

                    if (_env.IsDevelopment())
                    {
                        spa.UseReactDevelopmentServer(npmScript: "start");
                    }
                });
            });
        }
    }
}
