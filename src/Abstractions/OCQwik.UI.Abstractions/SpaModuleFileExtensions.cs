using System;
using System.Collections.Generic;
using System.Linq;
using Buzz.OrchardCore.SpaServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using OrchardCore.Modules;

namespace OCQwik.UI.Abstractions
{
    public static class SpaModuleFileExtensions
    {
        public static void AddModuleSpaStaticFiles(
           this IServiceCollection services,
           string moduleName,
           Action<SpaStaticFilesOptions> configuration = null)
        {
            services.Configure<Dictionary<string, Action<SpaStaticFilesOptions>>>(opts => opts.Add(moduleName, configuration));

            //It should matter if this is called multiple times as it's only registering the factory method
            services.AddSingleton<ISpaModuleFileProvider>(serviceProvider =>
            {

                var moduleConfigurations = serviceProvider.GetService<IOptions<Dictionary<string, Action<SpaStaticFilesOptions>>>>();

                moduleConfigurations.Value.ToDictionary(c => c.Key, c => new SpaStaticFilesOptions());
                var modulesOptions = moduleConfigurations.Value.ToDictionary(moduleConfiguration => moduleConfiguration.Key, moduleConfiguration =>
                {
                    // Use the options configured in DI (or blank if none was configured)
                    var optionsProvider = serviceProvider.GetService<IOptions<SpaStaticFilesOptions>>();
                    var options = optionsProvider.Value;

                    //we need to clone the default options
                    var moduleOptions = new SpaStaticFilesOptions()
                    {
                        RootPath = options.RootPath
                    };

                    //Allow the developer to perform further configuration
                    moduleConfiguration.Value?.Invoke(moduleOptions);

                    if (string.IsNullOrEmpty(moduleOptions.RootPath))
                    {
                        throw new InvalidOperationException($"No {nameof(SpaStaticFilesOptions.RootPath)} " +
                            $"was set on the {nameof(SpaStaticFilesOptions)}.");
                    }

                    return moduleOptions;
                });


                return new DefaultSpaModuleFileProvider(serviceProvider.GetService<IApplicationContext>(), modulesOptions);
            });

        }




        /// <summary>
        /// Configures the application to serve static files for a Single Page Application (SPA).
        /// The files will be located using the registered <see cref="ISpaStaticFileProvider"/> service.
        /// </summary>
        /// <param name="applicationBuilder">The <see cref="IApplicationBuilder"/>.</param>
        /// <param name="moduleName">The module name</param>
        public static void UseModuleSpaStaticFiles(this IApplicationBuilder applicationBuilder, string moduleName)
        {
            UseModuleSpaStaticFiles(applicationBuilder, moduleName, new StaticFileOptions());
        }

        /// <summary>
        /// Configures the application to serve static files for a Single Page Application (SPA).
        /// The files will be located using the registered <see cref="ISpaStaticFileProvider"/> service.
        /// </summary>
        /// <param name="applicationBuilder">The <see cref="IApplicationBuilder"/>.</param>
        /// <param name="moduleName">The module name</param>
        /// <param name="options">Specifies options for serving the static files.</param>
        public static void UseModuleSpaStaticFiles(this IApplicationBuilder applicationBuilder, string moduleName, StaticFileOptions options)
        {
            if (applicationBuilder == null)
            {
                throw new ArgumentNullException(nameof(applicationBuilder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            UseModuleSpaStaticFilesInternal(applicationBuilder,
                moduleName,
                staticFileOptions: options,
                allowFallbackOnServingWebRootFiles: false);
        }

        internal static void UseModuleSpaStaticFilesInternal(
            this IApplicationBuilder app,
            string moduleName,
            StaticFileOptions staticFileOptions,
            bool allowFallbackOnServingWebRootFiles)
        {
            if (staticFileOptions == null)
            {
                throw new ArgumentNullException(nameof(staticFileOptions));
            }

            // If the file provider was explicitly supplied, that takes precedence over any other
            // configured file provider. This is most useful if the application hosts multiple SPAs
            // (via multiple calls to UseSpa()), so each needs to serve its own separate static files
            // instead of using AddSpaStaticFiles/UseSpaStaticFiles.
            // But if no file provider was specified, try to get one from the DI config.
            if (staticFileOptions.FileProvider == null)
            {
                var shouldServeStaticFiles = ShouldServeStaticFiles(
                    app,
                    moduleName,
                    allowFallbackOnServingWebRootFiles,
                    out var fileProviderOrDefault);
                if (shouldServeStaticFiles)
                {
                    staticFileOptions.FileProvider = fileProviderOrDefault;
                }
                else
                {
                    // The registered ISpaStaticFileProvider says we shouldn't
                    // serve static files
                    return;
                }
            }

            app.UseStaticFiles(staticFileOptions);
        }

        private static bool ShouldServeStaticFiles(
            IApplicationBuilder app,
            string moduleName,
            bool allowFallbackOnServingWebRootFiles,
            out IFileProvider fileProviderOrDefault)
        {
            var spaStaticFilesService = app.ApplicationServices.GetService<ISpaModuleFileProvider>();
            if (spaStaticFilesService != null)
            {
                // If an ISpaStaticFileProvider was configured but it says no IFileProvider is available
                // (i.e., it supplies 'null'), this implies we should not serve any static files. This
                // is typically the case in development when SPA static files are being served from a
                // SPA development server (e.g., Angular CLI or create-react-app), in which case no
                // directory of prebuilt files will exist on disk.
                fileProviderOrDefault = spaStaticFilesService.GetModuleFileProvider(moduleName);
                return fileProviderOrDefault != null;
            }
            else if (!allowFallbackOnServingWebRootFiles)
            {
                throw new InvalidOperationException($"To use {nameof(UseModuleSpaStaticFiles)}, you must " +
                    $"first register an {nameof(ISpaModuleFileProvider)} in the service provider, typically " +
                    $"by calling services.{nameof(AddModuleSpaStaticFiles)}.");
            }
            else
            {
                // Fall back on serving wwwroot
                fileProviderOrDefault = null;
                return true;
            }
        }
    }
}
