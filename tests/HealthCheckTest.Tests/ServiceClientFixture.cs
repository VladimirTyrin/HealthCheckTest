using Xunit;

namespace HealthCheckTest.Tests
{
    [CollectionDefinition(Constants.IntegrationTests)]
    public class ServiceClientFixture : IClassFixture<ServiceClient>
    {

    }
}