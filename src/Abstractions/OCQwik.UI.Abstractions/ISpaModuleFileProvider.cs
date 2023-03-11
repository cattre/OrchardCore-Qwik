using Microsoft.Extensions.FileProviders;

namespace Buzz.OrchardCore.SpaServices
{
    public interface ISpaModuleFileProvider
    {
        IFileProvider GetModuleFileProvider(string moduleName);
    }
}
