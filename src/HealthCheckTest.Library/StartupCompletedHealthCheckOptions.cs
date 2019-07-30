using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckTest.Library
{
    public class StartupCompletedHealthCheckOptions
    {
        /// <summary>
        ///     If true, health check will not return until Startup.Configure completion
        ///     If false, check will return <see cref="HealthCheckResult.Degraded"/>
        /// </summary>
        public bool UseLongPolling { get; set; }
    }
}