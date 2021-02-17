using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Modules;
using DotNet.Testcontainers.Containers.Modules.Databases;
using Xunit;

namespace Armut.Tests.Common.Fixtures
{
    public class DatabaseFixture : IAsyncLifetime
    {
        private readonly PostgreSqlTestcontainer _postgreSqlTestcontainer;
        private readonly TestcontainersContainer _pgadminTestContainer;

        public DatabaseFixture()
        {
            var databaseBuilder = new TestcontainersBuilder<PostgreSqlTestcontainer>()
                .WithName($"postgres-integration-{DateTime.Now.Ticks}")
                .WithCleanUp(true)
                .WithDatabase(new PostgreSqlTestcontainerConfiguration("postgres:12.6")
                {
                    Password = "Pass@word",
                    Database = "armutdb",
                    Username = "postgres",
                    Port = 5432
                })
                .WithEnvironment("DOCKER_GATEWAY_HOST", "172.17.0.1");

            var pgadminTestContainerBuilder = new TestcontainersBuilder<TestcontainersContainer>()
                .WithName($"pgadmin4-integration-{DateTime.Now.Ticks}")
                .WithImage("dpage/pgadmin4:4.30")
                .WithCleanUp(true)
                .WithEnvironment("PGADMIN_DEFAULT_EMAIL", "pgadmin4@pgadmin.org")
                .WithEnvironment("PGADMIN_DEFAULT_PASSWORD", "admin")
                .WithEnvironment("DOCKER_GATEWAY_HOST", "172.17.0.1")
                .WithPortBinding(5050, 80);

            _postgreSqlTestcontainer = databaseBuilder.Build();
            _pgadminTestContainer = pgadminTestContainerBuilder.Build();
        }

        public async Task InitializeAsync()
        {
            await _postgreSqlTestcontainer.StartAsync();
            await _pgadminTestContainer.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _postgreSqlTestcontainer.StopAsync();
            await _pgadminTestContainer.StopAsync();
        }
    }
}
