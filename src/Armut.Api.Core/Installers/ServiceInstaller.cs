using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Armut.Api.Core.Components;
using Armut.Api.Core.Contracts;
using Armut.Api.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Armut.Api.Core.Installers
{
    public class ServiceInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Assembly assembly = typeof(UserService).Assembly;

            foreach (Type type in assembly.GetTypes())
            {
                bool isComponent = type.GetInterfaces().Contains(typeof(IService));

                if (!isComponent)
                {
                    continue;
                }

                Type componentInterfaceType = type.GetInterfaces().FirstOrDefault(interfaceType =>
                    interfaceType.GetInterfaces().Contains(typeof(IService)) &&
                    interfaceType.GetInterfaces().Length == 1);

                if (componentInterfaceType != null)
                {
                    services.AddTransient(componentInterfaceType, type);
                }
            }

            services.AddTransient<IModelValidator, ModelValidator>();
        }
    }
}