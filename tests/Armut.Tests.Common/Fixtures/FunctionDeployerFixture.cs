using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules;
using DotNet.Testcontainers.Images.Builders;
using Xunit;

namespace Armut.Tests.Common.Fixtures
{
    public class FunctionDeployerFixture : IAsyncLifetime
    {
        private readonly TestcontainersContainer _functionDeployerContainer;

        public FunctionDeployerFixture()
        {
            DirectoryInfo slnDirectory = TryGetSolutionDirectoryInfo();
            string dockerFileDirectory = Path.Combine(slnDirectory.FullName, "src");

            var functionDeployerBuilder = new ImageFromDockerfileBuilder()
                .WithName("event-processor-deployer")
                .WithDockerfileDirectory(dockerFileDirectory)
                .WithDeleteIfExists(true);

            string result = functionDeployerBuilder.Build().GetAwaiter().GetResult();

            ITestcontainersBuilder<TestcontainersContainer> functionDeployerContainer =
                new TestcontainersBuilder<TestcontainersContainer>()
                    .WithName("event-processor-deployer")
                    .WithImage(result)
                    .WithCleanUp(true)
                    .WithEnvironment("DOCKER_GATEWAY_HOST", "172.17.0.1")
                    .WithEnvironment("LOCALSTACK_HOST", "172.17.0.1")
                    .WithEnvironment("SERVERLESS_STAGE", "local")
                    .WithEnvironment("CONN_STR", "host=172.17.0.1;port=5432;database=armutdb;username=postgres;password=Pass@word;Timeout=30");

            _functionDeployerContainer = functionDeployerContainer.Build();
        }

        public async Task InitializeAsync()
        {
            await _functionDeployerContainer.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _functionDeployerContainer.StopAsync();
        }

        public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
        {
            var directory = new DirectoryInfo(currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }
    }
}
