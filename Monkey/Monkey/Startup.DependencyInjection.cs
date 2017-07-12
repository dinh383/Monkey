using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Puppy.DependencyInjection;
using System.IO;

namespace Monkey
{
    public partial class Startup
    {
        public static class DependencyInjection
        {
            public static void Service(IServiceCollection services)
            {
                var systemPrefix = $"{nameof(Monkey)}";
                services
                    .AddDependencyInjectionScanner()
                    .ScanFromAllAssemblies($"{systemPrefix}.*.dll",
                        Path.GetFullPath(PlatformServices.Default.Application.ApplicationBasePath));

                // Write out all dependency injection services
                services.WriteOut(systemPrefix);
            }
        }
    }
}