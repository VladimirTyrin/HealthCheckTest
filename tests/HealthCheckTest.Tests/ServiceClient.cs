using System;
using System.Net.Http;
using HealthCheckTest.Application;
using Microsoft.AspNetCore.TestHost;

namespace HealthCheckTest.Tests
{
    public class ServiceClient : IDisposable
    {
        public readonly HttpClient Client;
        private readonly TestServer _testServer;

        public ServiceClient()
        {
            _testServer = new TestServer(Program.CreateWebHostBuilder(new string[0]));
            Client = _testServer.CreateClient();
        }

        public void Dispose()
        {
            _testServer.Host.StopAsync().Wait();
            _testServer.Dispose();
        }
    }
}