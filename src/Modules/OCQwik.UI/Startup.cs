using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OCQwik.UI.Abstractions;
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
            services.AddModuleSpaStaticFiles("OCQwik.UI", opts => opts.RootPath = "/ClientApp/dist/build");

            // services.AddSpaStaticFiles(spa => spa.RootPath = "/ClientApp/dist/");
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            // if (_env.IsDevelopment())
            // {
            //     builder.UseDeveloperExceptionPage();
            // }
            //
            // builder.UseSpaStaticFiles();

            builder.UseRewriter(new Microsoft.AspNetCore.Rewrite.RewriteOptions()
                .AddRedirect("^$", "/app"));

            if (_env.IsDevelopment())
            {
                builder.MapWhen(context => context.Request.Path.StartsWithSegments("/app"), spaApp =>
                {
                    spaApp.UseModuleSpa("OCQwik.UI", spa =>
                    {
                        spa.Options.SourcePath = "../Modules/OCQwik.UI/ClientApp";
                        spa.Options.PackageManagerCommand = "pnpm";
                        spa.UseProxyToSpaDevelopmentServer("http://localhost:5173/");
                    });
                });
            }
            else
            {
                builder.Map("/app", spaApp =>
                {
                    spaApp.UseModuleSpaStaticFiles("OCQwik.UI");
                    spaApp.UseModuleSpa("OCQwik.UI", spa =>
                    {
                        spa.Options.SourcePath = "../Modules/OCQwik.UI/ClientApp";
                    });
                });
            }

            // builder.Map("/app", spaApp =>
            // {
            //     spaApp.UseSpa(spa =>
            //     {
            //         // spa.Options.SourcePath = Path.Join(_env.ContentRootPath, "ClientApp");
            //
            //         spa.Options.SourcePath = "/ClientApp";
            //
            //         if (_env.IsDevelopment())
            //         {
            //             spa.UseProxyToSpaDevelopmentServer("http://localhost:5173/");
            //         }
            //     });
            // });
        }
    }
}
