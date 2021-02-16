using Armut.Tests.Common.Fixtures;
using Xunit;

namespace Armut.Api.Core.IntegrationTests.CollectionDefinitions
{
    [CollectionDefinition(nameof(IntegrationTestCollection))]
    public class IntegrationTestCollection : ICollectionFixture<DatabaseFixture>, ICollectionFixture<IntegrationTestFixture>
    {
    }
}
