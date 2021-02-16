using Armut.Api.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Armut.Api.Core.Installers
{
    public class EFCoreInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            string connectionString = configuration.GetConnectionString("Database");

            services.AddDbContextPool<ArmutContext>(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.UseNpgsql(connectionString);
            });
        }
    }
}
