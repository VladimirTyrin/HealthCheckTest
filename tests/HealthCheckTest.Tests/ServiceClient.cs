using System.Net.Http;
using HealthCheckTest.Application;
using Microsoft.AspNetCore.TestHost;

namespace HealthCheckTest.Tests
{
    public class ServiceClient
    {
        public readonly HttpClient Client;

        public ServiceClient()
        {
            var testServer = new TestServer(Program.CreateWebHostBuilder(new string[0]));
            Client = testServer.CreateClient();
        }
    }
}