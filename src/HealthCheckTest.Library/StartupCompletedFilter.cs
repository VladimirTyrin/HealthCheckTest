using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace HealthCheckTest.Library
{
    internal class StartupCompletedFilter : IStartupFilter
    {
        private readonly IStartupCompletedCallback _completedCallback;

        public StartupCompletedFilter(IStartupCompletedCallback completedCallback)
        {
            _completedCallback = completedCallback;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    next(builder);
                    sw.Stop();
                    _completedCallback.SetStartupCompleted(sw.ElapsedMilliseconds);
                }
                catch (Exception e)
                {
                    _completedCallback.SetStartupFailed(e);
                    throw;
                }
            };
        }
    }
}