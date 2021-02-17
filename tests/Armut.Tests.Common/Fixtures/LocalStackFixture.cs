using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Armut.Tests.Common.ContainerBuilder;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations;
using DotNet.Testcontainers.Containers.Modules;
using Xunit;

namespace Armut.Tests.Common.Fixtures
{
    public class LocalStackFixture : IAsyncLifetime
    {
        private readonly TestcontainersContainer _localStackContainer;

        public LocalStackFixture()
        {
            ITestcontainersBuilder<TestcontainersContainer> localStackBuilder =
                new TestcontainersBuilder<TestcontainersContainer>()
                    .WithName($"LocalStack-0.12.6-{DateTime.Now.Ticks}")
                    .WithImage("localstack/localstack:0.12.6")
                    .WithMount("/var/run/docker.sock", "/var/run/docker.sock")
                    .WithCleanUp(true)
                    .WithEnvironment("DEFAULT_REGION", "eu-central-1")
                    .WithEnvironment("DATA_DIR", "/tmp/localstack/data")
                    .WithEnvironment("SERVICES", "iam,lambda,dynamodb,apigateway,s3,sns,cloudformation,cloudwatch,sts")
                    .WithEnvironment("DOCKER_HOST", "unix:///var/run/docker.sock")
                    .WithEnvironment("LAMBDA_EXECUTOR", "docker-reuse")
                    .WithEnvironment("LS_LOG", "debug")
                    .WithEnvironment("DOCKER_GATEWAY_HOST", "172.17.0.1")
                    //.WithEnvironment("LAMBDA_DOCKER_DNS", "172.17.0.1")
                    //.WithEnvironment("HOSTNAME", "172.17.0.1")
                    //.WithEnvironment("HOSTNAME_EXTERNAL", "172.17.0.1")
                    //.WithEnvironment("LOCALSTACK_HOSTNAME", "172.17.0.1")
                    //.WithEnvironment("MAIN_CONTAINER_NAME", "LocalStack-0.12.6")
                    .WithPortBinding(4566, 4566);

            _localStackContainer = localStackBuilder.Build();
            ReplaceMountValue(_localStackContainer, new List<IBind>() { new WslMount("/var/run/docker.sock", "/var/run/docker.sock", AccessMode.ReadWrite) });
        }
        public async Task InitializeAsync()
        {
            await _localStackContainer.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _localStackContainer.StopAsync();
        }


        private static void ReplaceMountValue(TestcontainersContainer testcontainersContainer, IEnumerable<IBind> wslMounts)
        {
            FieldInfo configurationFieldInfo = testcontainersContainer.GetType().GetField("configuration", BindingFlags.Instance | BindingFlags.NonPublic);
            var configuration = (ITestcontainersConfiguration) configurationFieldInfo.GetValue(testcontainersContainer);

            PropertyInfo propertyInfo = configuration
                .GetType()
                .GetProperty(nameof(configuration.Mounts), BindingFlags.Public | BindingFlags.Instance);

            FieldInfo backingField = GetBackingField(propertyInfo);

            backingField.SetValue(configuration, wslMounts);
        }

        private static FieldInfo GetBackingField(PropertyInfo pi) {
            if (!pi.CanRead || !pi.GetGetMethod(nonPublic:true).IsDefined(typeof(CompilerGeneratedAttribute), inherit:true))
                return null;
            var backingField = pi.DeclaringType.GetField($"<{pi.Name}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            if (backingField == null)
                return null;
            if (!backingField.IsDefined(typeof(CompilerGeneratedAttribute), inherit:true))
                return null;
            return backingField;
        }
    }
}
