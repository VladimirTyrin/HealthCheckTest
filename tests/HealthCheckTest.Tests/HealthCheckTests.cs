using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace HealthCheckTest.Tests
{
    [Collection(Constants.IntegrationTests)]
    public class HealthCheckTests
    {
        private readonly ServiceClient _serviceClient;
        private readonly ITestOutputHelper _outputHelper;

        public HealthCheckTests(ServiceClient serviceClient, ITestOutputHelper outputHelper)
        {
            _serviceClient = serviceClient;
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task HealthChecks_Return200()
        {
            using (var response = await _serviceClient.Client.GetAsync("/ready"))
            {
                response.EnsureSuccessStatusCode();

                _outputHelper.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
