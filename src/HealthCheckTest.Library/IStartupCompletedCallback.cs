using System;

namespace HealthCheckTest.Library
{
    /// <summary>
    ///     Methods to be invoked after Startup.Configure completion
    /// </summary>
    internal interface IStartupCompletedCallback
    {
        void SetStartupCompleted(long milliseconds);

        void SetStartupFailed(Exception exception);
    }
}