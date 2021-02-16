using System.IO;
using Armut.Api.Core;
using Armut.Api.Core.Installers;
using Armut.Api.Core.Models.Validators;
using Armut.Tests.Common.Components;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace Armut.Tests.Common.Fixtures
{
    public class IntegrationTestFixture
    {
        public ConfigurationBuilder CreateConfigureAppConfiguration(string configFile = null)
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json", optional: true);
            if (!string.IsNullOrEmpty(configFile))
            {
                builder.AddJsonFile(configFile, optional: true);
            }
            builder.AddEnvironmentVariables();

            return builder;
        }

        public IServiceCollection CreateServiceCollection(IConfiguration configuration)
        {
            var serviceCollection = new ServiceCollection();

            var mockEnvironment = new Mock<IHostEnvironment>();
            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Hosting:UnitTestEnvironment");

            //serviceCollection.AddLocalStack(configuration);

            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            serviceCollection
                .AddAutoMapper(typeof(MappingProfile))
                .AddTransient<IValidatorFactory, TestValidatorFactory>()
                .AddValidatorsFromAssemblyContaining<AddUserModelValidator>()
                .InstallServices(configuration, mockEnvironment.Object);

            return serviceCollection;
        }
    }
}
