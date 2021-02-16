﻿using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules;
using Xunit;

namespace Armut.Tests.Common.Fixtures
{
    public class LocalStackFixture : IAsyncLifetime
    {
        private readonly TestcontainersContainer _localStackContainer;

        public LocalStackFixture()
        {
            ITestcontainersBuilder<TestcontainersContainer> localStackBuilder = new TestcontainersBuilder<TestcontainersContainer>()
                .WithName("LocalStack-0.12.6")
                .WithImage("localstack/localstack:0.12.6")
                .WithCleanUp(true)
                .WithEnvironment("DEFAULT_REGION", "eu-central-1")
                .WithEnvironment("SERVICES", "s3")
                .WithEnvironment("DOCKER_HOST", "unix:///var/run/docker.sock")
                .WithEnvironment("LS_LOG", "info")
                .WithPortBinding(4566, 4566)
                .WithEnvironment("DOCKER_GATEWAY_HOST", "172.17.0.1");

            _localStackContainer = localStackBuilder.Build();
        }
        public async Task InitializeAsync()
        {
            await _localStackContainer.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _localStackContainer.StopAsync();
        }
    }
}
