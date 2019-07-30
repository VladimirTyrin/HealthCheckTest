using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckTest.Library
{
    /// <summary>
    ///     Health check that returns 200 after Startup.Configure completion
    /// </summary>
    internal class StartupCompletedHealthCheck : IHealthCheck, IStartupCompletedCallback
    {
        private readonly TaskCompletionSource<HealthCheckResult> _longPollingTaskCompletionSource = new TaskCompletionSource<HealthCheckResult>();
        private readonly StartupCompletedHealthCheckOptions _options;
        private readonly object _stateLock = new object();

        private bool _completed;
        private bool _completedSuccessfully;
        private Exception _exception;
        private IReadOnlyDictionary<string, object> _completionData;

        public StartupCompletedHealthCheck(StartupCompletedHealthCheckOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _options.UseLongPolling
                ? _longPollingTaskCompletionSource.Task
                : Task.FromResult(ImmediateState());
        }

        public void SetStartupCompleted(long milliseconds)
        {
            if (_completed)
                return;

            lock (_stateLock)
            {
                if (_completed)
                    return;

                _completed = true;
                _completedSuccessfully = true;
                _completionData =  new Dictionary<string, object>
                {
                    ["STARTUP_TIME"] = milliseconds
                };
            }

            CompleteLongPolling();
        }

        public void SetStartupFailed(Exception exception)
        {
            if (_completed)
                return;

            lock (_stateLock)
            {
                if (_completed)
                    return;

                _completed = true;
                _completedSuccessfully = false;
                _exception = exception;
            }

            CompleteLongPolling();
        }

        private void CompleteLongPolling()
        {
            if (! _options.UseLongPolling)
                return;

            // We know for sure startup is completed (maybe unsuccessfully)
            _longPollingTaskCompletionSource.TrySetResult(ImmediateState());
        }

        private HealthCheckResult ImmediateState()
        {
            if (! _completed)
                return HealthCheckResult.Degraded("Startup is not yet completed");

            if (_completedSuccessfully)
                return HealthCheckResult.Healthy("OK", _completionData);

            return HealthCheckResult.Unhealthy("Startup error", _exception);
        }
    }
}