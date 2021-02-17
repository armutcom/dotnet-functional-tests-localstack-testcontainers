using Armut.Tests.Common.Fixtures;
using Xunit;

namespace Armut.Api.FunctionalTests.CollectionDefinitions
{
    [CollectionDefinition(nameof(ApiTestCollection))]
    public class ApiTestCollection : ICollectionFixture<DatabaseFixture>, ICollectionFixture<LocalStackFixture>, ICollectionFixture<FunctionDeployerFixture>, ICollectionFixture<TestServerFixture>
    {
    }

    //[CollectionDefinition(nameof(ApiTestCollection))]
    //public class ApiTestCollection : ICollectionFixture<DatabaseFixture>, ICollectionFixture<FunctionDeployerFixture>, ICollectionFixture<TestServerFixture>
    //{
    //}
}