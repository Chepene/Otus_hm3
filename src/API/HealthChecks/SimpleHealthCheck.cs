using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace API.HealthChecks
{
    public class DumbHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Healthy("Dumb check is ok"));
        }

    }
}