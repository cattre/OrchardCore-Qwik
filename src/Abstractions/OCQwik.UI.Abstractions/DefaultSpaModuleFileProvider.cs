using System;
using System.Collections.Generic;
using Buzz.OrchardCore.SpaServices;
using Microsoft.AspNetCore.SpaServices.StaticFiles;
using Microsoft.Extensions.FileProviders;
using OrchardCore.Modules;

namespace OCQwik.UI.Abstractions
{
    public class DefaultSpaModuleFileProvider : ISpaModuleFileProvider
    {
        private readonly IDictionary<string, IFileProvider> _fileProviders;

        public DefaultSpaModuleFileProvider(IApplicationContext context, IDictionary<string, SpaStaticFilesOptions> modulesOptions)
        {
            if (modulesOptions == null)
            {
                throw new ArgumentNullException(nameof(modulesOptions));
            }

            var fileProviders = new Dictionary<string, IFileProvider>();

            foreach (var moduleKey in modulesOptions.Keys)
            {
                var options = modulesOptions[moduleKey];

                if (string.IsNullOrEmpty(options.RootPath))
                {
                    throw new ArgumentException($"The {nameof(options.RootPath)} property " +
                        $"of {nameof(options)} cannot be null or empty for module {moduleKey}");
                }

                fileProviders[moduleKey] = new SpaModuleEmbeddedFileProvider(context, moduleKey, options.RootPath);
            }

            _fileProviders = fileProviders;
        }

        public IFileProvider GetModuleFileProvider(string moduleName)
        {
            return _fileProviders[moduleName];
        }
    }
}
