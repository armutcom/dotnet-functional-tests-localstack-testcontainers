using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Armut.Api.Core.Contracts
{
    public interface IInstaller
    {
        void Install(IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment);
    }
}
