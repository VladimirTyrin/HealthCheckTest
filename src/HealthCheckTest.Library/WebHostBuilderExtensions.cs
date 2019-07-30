using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HealthCheckTest.Library
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseStartupCompletedHealthCheck(this IWebHostBuilder builder)
        {
            return builder.UseStartupCompletedHealthCheck(_ => { });
        }

        public static IWebHostBuilder UseStartupCompletedHealthCheck(this IWebHostBuilder builder, Action<StartupCompletedHealthCheckOptions> configureOptions)
        {
            var options = new StartupCompletedHealthCheckOptions();
            configureOptions?.Invoke(options);
            var healthCheck = new StartupCompletedHealthCheck(options);

            builder.ConfigureServices(services =>
            {
                services.TryAddSingleton<IStartupCompletedCallback>(healthCheck);

                services
                    .AddHealthChecks()
                    .AddCheck(nameof(StartupCompletedHealthCheck), healthCheck, null, new []
                    {
                        "live",
                        "ready"
                    });

                services.AddTransient<IStartupFilter, StartupCompletedFilter>();
            });
            return builder;
        }
    }
}
