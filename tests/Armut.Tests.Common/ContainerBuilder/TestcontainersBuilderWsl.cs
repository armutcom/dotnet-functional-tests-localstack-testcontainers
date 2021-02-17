using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNet.Testcontainers.Clients;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations;
using DotNet.Testcontainers.Containers.OutputConsumers;
using DotNet.Testcontainers.Containers.WaitStrategies;
using DotNet.Testcontainers.Images;

namespace Armut.Tests.Common.ContainerBuilder
{
    public class TestcontainersBuilderWsl<TDockerContainer> : ITestcontainersBuilder<TDockerContainer>
        where TDockerContainer : IDockerContainer
    {
        private readonly TestcontainersBuilder<TDockerContainer> _testcontainersBuilder;

        public TestcontainersBuilderWsl()
        {
            _testcontainersBuilder = new TestcontainersBuilder<TDockerContainer>();
        }

        public ITestcontainersBuilder<TDockerContainer> ConfigureContainer(Action<TDockerContainer> moduleConfiguration)
        {
            _testcontainersBuilder.ConfigureContainer(moduleConfiguration);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithImage(string image)
        {
            _testcontainersBuilder.WithImage(image);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithImage(IDockerImage image)
        {
            _testcontainersBuilder.WithImage(image);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithName(string name)
        {
            _testcontainersBuilder.WithName(name);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithWorkingDirectory(string workingDirectory)
        {
            _testcontainersBuilder.WithWorkingDirectory(workingDirectory);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithEntrypoint(params string[] entrypoint)
        {
            _testcontainersBuilder.WithEntrypoint(entrypoint);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithCommand(params string[] command)
        {
            _testcontainersBuilder.WithCommand(command);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithEnvironment(string name, string value)
        {
            _testcontainersBuilder.WithEnvironment(name, value);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithLabel(string name, string value)
        {
            _testcontainersBuilder.WithLabel(name, value);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithExposedPort(int port)
        {
            _testcontainersBuilder.WithExposedPort(port);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithExposedPort(string port)
        {
            _testcontainersBuilder.WithExposedPort(port);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithPortBinding(int port, bool assignRandomHostPort = false)
        {
            _testcontainersBuilder.WithPortBinding(port, assignRandomHostPort);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithPortBinding(int hostPort, int containerPort)
        {
            _testcontainersBuilder.WithPortBinding(hostPort, containerPort);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithPortBinding(string port, bool assignRandomHostPort = false)
        {
            _testcontainersBuilder.WithPortBinding(port, assignRandomHostPort);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithPortBinding(string hostPort, string containerPort)
        {
            return _testcontainersBuilder.WithPortBinding(hostPort, containerPort);
        }

        public ITestcontainersBuilder<TDockerContainer> WithMount(string source, string destination)
        {
            var mounts = new IBind[] {new WslMount(source, destination, AccessMode.ReadWrite)};
            return Build(_testcontainersBuilder, Apply(mounts: mounts));
        }

        public ITestcontainersBuilder<TDockerContainer> WithCleanUp(bool cleanUp)
        {
            _testcontainersBuilder.WithCleanUp(cleanUp);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithDockerEndpoint(string endpoint)
        {
            _testcontainersBuilder.WithDockerEndpoint(endpoint);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithRegistryAuthentication(string registryEndpoint,
            string username, string password)
        {
            _testcontainersBuilder.WithRegistryAuthentication(registryEndpoint, username, password);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithOutputConsumer(IOutputConsumer outputConsumer)
        {
            _testcontainersBuilder.WithOutputConsumer(outputConsumer);
            return this;
        }

        public ITestcontainersBuilder<TDockerContainer> WithWaitStrategy(IWaitForContainerOS waitStrategy)
        {
            _testcontainersBuilder.WithWaitStrategy(waitStrategy);
            return this;
        }

        public TDockerContainer Build()
        {
            return _testcontainersBuilder.Build();
        }

        private static ITestcontainersConfiguration Apply(
            Uri endpoint = null,
            IAuthenticationConfiguration authConfig = null,
            IDockerImage image = null,
            string name = null,
            string workingDirectory = null,
            IEnumerable<string> entrypoint = null,
            IEnumerable<string> command = null,
            IReadOnlyDictionary<string, string> environments = null,
            IReadOnlyDictionary<string, string> labels = null,
            IReadOnlyDictionary<string, string> exposedPorts = null,
            IReadOnlyDictionary<string, string> portBindings = null,
            IEnumerable<IBind> mounts = null,
            IOutputConsumer outputConsumer = null,
            IEnumerable<IWaitUntil> waitStrategies = null,
            bool cleanUp = true)
        {
            Type dockerApiEndpointType = typeof(TestcontainersConfiguration).Assembly.GetType("DotNet.Testcontainers.Clients.DockerApiEndpoint");
            PropertyInfo localProperty = dockerApiEndpointType.GetProperties()[0];
            var localEndpoint = localProperty.GetValue(null) as Uri;


            return new TestcontainersConfiguration(
                endpoint ?? localEndpoint,
                authConfig,
                image,
                name,
                workingDirectory,
                entrypoint,
                command,
                environments,
                labels,
                exposedPorts,
                portBindings,
                mounts,
                outputConsumer,
                waitStrategies,
                cleanUp);
        }

        private static ITestcontainersBuilder<TDockerContainer> Build(
            TestcontainersBuilder<TDockerContainer> previous,
            ITestcontainersConfiguration next,
            Action<TDockerContainer> moduleConfiguration = null)
        {
            Type dockerApiEndpointType = typeof(TestcontainersConfiguration).Assembly.GetType("DotNet.Testcontainers.Clients.DockerApiEndpoint");
            PropertyInfo localProperty = dockerApiEndpointType.GetProperties()[0];
            var localEndpoint = localProperty.GetValue(null) as Uri;

            FieldInfo configurationFieldInfo = previous.GetType().GetField("configuration", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo moduleConfigurationFieldInfo = previous.GetType().GetField("moduleConfiguration", BindingFlags.Instance | BindingFlags.NonPublic);

            var previousConfiguration = (ITestcontainersConfiguration) configurationFieldInfo.GetValue(previous);
            var previousModuleConfiguration = (Action<TDockerContainer>) moduleConfigurationFieldInfo.GetValue(previous);

            var cleanUp = next.CleanUp && previousConfiguration.CleanUp;
            var endpoint = Merge(next.Endpoint, previousConfiguration.Endpoint, localEndpoint);
            var image = Merge(next.Image, previousConfiguration.Image);
            var name = Merge(next.Name, previousConfiguration.Name);
            var workingDirectory = Merge(next.WorkingDirectory, previousConfiguration.WorkingDirectory);
            var entrypoint = Merge(next.Entrypoint, previousConfiguration.Entrypoint);
            var command = Merge(next.Command, previousConfiguration.Command);
            var environments = Merge(next.Environments, previousConfiguration.Environments);
            var labels = Merge(next.Labels, previousConfiguration.Labels);
            var exposedPorts = Merge(next.ExposedPorts, previousConfiguration.ExposedPorts);
            var portBindings = Merge(next.PortBindings, previousConfiguration.PortBindings);
            var mounts = Merge(next.Mounts, previousConfiguration.Mounts);

            var authConfig = new[] {next.AuthConfig, previousConfiguration.AuthConfig}.First(config => config != null);
            var outputConsumer =
                new[] {next.OutputConsumer, previousConfiguration.OutputConsumer}.First(config => config != null);
            var waitStrategies =
                new[] {next.WaitStrategies, previousConfiguration.WaitStrategies}.First(config => config != null);

            var mergedConfiguration = Apply(
                endpoint,
                authConfig,
                image,
                name,
                workingDirectory,
                entrypoint,
                command,
                environments,
                labels,
                exposedPorts,
                portBindings,
                mounts,
                outputConsumer,
                waitStrategies,
                cleanUp);

            ConstructorInfo constructorInfo = typeof(TestcontainersBuilder<TDockerContainer>).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance, null,
                new[] {typeof(ITestcontainersConfiguration), typeof(Action<TDockerContainer>)}, null);

            var testcontainersBuilder = (TestcontainersBuilder<TDockerContainer>)constructorInfo.Invoke(new object[] {mergedConfiguration, moduleConfiguration ?? previousModuleConfiguration});

            return testcontainersBuilder;
        }

        /// <summary>
        /// Returns the changed Testcontainer configuration object. If there is no change, the previous Testcontainer configuration object is returned.
        /// </summary>
        /// <param name="next">Changed Testcontainer configuration object.</param>
        /// <param name="previous">Previous Testcontainer configuration object.</param>
        /// <param name="defaultConfiguration">Default Testcontainer configuration.</param>
        /// <typeparam name="T">Any class.</typeparam>
        /// <returns>Changed Testcontainer configuration object. If there is no change, the previous Testcontainer configuration object.</returns>
        private static T Merge<T>(T next, T previous, T defaultConfiguration = null)
            where T : class
        {
            return next == null || next.Equals(defaultConfiguration) ? previous : next;
        }

        /// <summary>
        /// Merges all existing and new Testcontainer configuration changes. If there are no changes, the previous Testcontainer configurations are returned.
        /// </summary>
        /// <param name="next">Changed Testcontainer configuration.</param>
        /// <param name="previous">Previous Testcontainer configuration.</param>
        /// <typeparam name="T">Type of <see cref="IReadOnlyDictionary{TKey,TValue}" />.</typeparam>
        /// <returns>An updated Testcontainer configuration.</returns>
        private static IEnumerable<T> Merge<T>(IEnumerable<T> next, IEnumerable<T> previous)
            where T : class
        {
            if (next == null || previous == null)
            {
                return next ?? previous;
            }
            else
            {
                return next.Concat(previous).ToArray();
            }
        }

        /// <summary>
        /// Merges all existing and new Testcontainer configuration changes. If there are no changes, the previous Testcontainer configurations are returned.
        /// </summary>
        /// <param name="next">Changed Testcontainer configuration.</param>
        /// <param name="previous">Previous Testcontainer configuration.</param>
        /// <typeparam name="T">Type of <see cref="IReadOnlyDictionary{TKey,TValue}" />.</typeparam>
        /// <returns>An updated Testcontainer configuration.</returns>
        private static IReadOnlyDictionary<T, T> Merge<T>(IReadOnlyDictionary<T, T> next,
            IReadOnlyDictionary<T, T> previous)
            where T : class
        {
            if (next == null || previous == null)
            {
                return next ?? previous;
            }
            else
            {
                return next.Concat(previous.Where(item => !next.Keys.Contains(item.Key)))
                    .ToDictionary(item => item.Key, item => item.Value);
            }
        }
    }
}
