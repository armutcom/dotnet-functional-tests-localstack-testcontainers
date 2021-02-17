using Armut.Api.Core.Contracts;
using Armut.Tests.Common.Fixtures;
using Armut.Tests.Common.Seeders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Armut.Api.Core.IntegrationTests
{
    public abstract class BaseTest
    {
        protected BaseTest(IntegrationTestFixture integrationTestFixture)
        {
            ConfigurationBuilder configurationBuilder = integrationTestFixture.CreateConfigureAppConfiguration();
            Configuration = configurationBuilder.Build();

            IServiceCollection serviceCollection = integrationTestFixture.CreateServiceCollection(Configuration);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            ArmutContext = ServiceProvider.GetRequiredService<ArmutContext>();

            ArmutContext.Database.EnsureCreated();
            Seeder.Seed(ArmutContext);

            UserService = ServiceProvider.GetRequiredService<IUserService>();
            EventService = ServiceProvider.GetRequiredService<IEventService>();
        }

        protected IConfiguration Configuration { get; }

        protected ServiceProvider ServiceProvider { get; }

        protected ArmutContext ArmutContext { get; }

        protected IUserService UserService { get; }

        protected IEventService EventService { get; }
    }
}
