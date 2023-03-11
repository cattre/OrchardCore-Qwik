using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using OrchardCore.Modules;

namespace OCQwik.UI.Abstractions
{
    public class SpaModuleEmbeddedFileProvider : IFileProvider
    {
        private readonly IApplicationContext _applicationContext;
        private Application Application => _applicationContext.Application;

        public string ModuleName { get; set; }
        public string Root { get; }

        public SpaModuleEmbeddedFileProvider(IApplicationContext context, string moduleName, string root)
        {
            _applicationContext = context;
            Root = root.StartsWith("/") ? root.Substring(1) : root;
            ModuleName = moduleName;
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return null;
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return Application.GetModule(ModuleName).GetFileInfo(GetFullPath(subpath));
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }

        private string GetFullPath(string path)
        {
            if (path.StartsWith("/"))
            {
                path = path.Substring(1);
            }

            if (string.IsNullOrEmpty(path))
            {
                path = "index.html";
            }

            string fullPath;
            try
            {
                fullPath = Path.Combine(Root, path);
            }
            catch
            {
                return null;
            }

            if (!IsUnderneathRoot(fullPath))
            {
                return null;
            }

            return fullPath;
        }

        private bool IsUnderneathRoot(string fullPath)
        {
            return fullPath.StartsWith(Root, StringComparison.OrdinalIgnoreCase);
        }
    }
}
