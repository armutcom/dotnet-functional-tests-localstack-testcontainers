using System;
using System.Linq;
using Armut.Api.Core.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Armut.Api.Core.Installers
{
    public static class InstallerExtensions
    {
        public static void InstallServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            var installers = typeof(InstallerExtensions).Assembly.ExportedTypes
                .Where(m => typeof(IInstaller).IsAssignableFrom(m) && !m.IsInterface && !m.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();

            installers.ForEach(m => m.Install(services, configuration, hostEnvironment));
        }
    }
}
