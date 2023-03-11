using System;
using Buzz.OrchardCore.SpaServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.DependencyInjection;

namespace OCQwik.UI.Abstractions
{
    public static class SpaModuleApplicationBuilderExtensions
    {
        public static void UseModuleSpa(this IApplicationBuilder app, string moduleName, Action<ISpaBuilder> configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var spaModuleFilesProvider = app.ApplicationServices.GetService<ISpaModuleFileProvider>();
            var spaModuleStaticFileProvider = spaModuleFilesProvider.GetModuleFileProvider(moduleName);

            app.UseSpa(spa =>
            {
                configuration.Invoke(spa);
                spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions { FileProvider = spaModuleStaticFileProvider };
            });
        }
    }
}
