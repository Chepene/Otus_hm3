using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace API.HealthChecks
{
    public class ConfigurationHealthCheck : IHealthCheck
    {
       private readonly IConfiguration _configuration;

       public ConfigurationHealthCheck(IConfiguration configuration)
       {
            _configuration = configuration;        
       }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (_configuration == null)
                Task.FromResult(HealthCheckResult.Unhealthy("config is null"));

            var errs = new List<string>();

            var host = _configuration.GetValue<string>("DBHOSTNAME");
            if (string.IsNullOrEmpty(host))
                errs.Add("host is empty");

            var port = _configuration.GetValue<string>("DBPORT");
            if (string.IsNullOrEmpty(port))
                errs.Add("port is empty");

            var dbName = _configuration.GetValue<string>("DBNAME");
            if (string.IsNullOrEmpty(dbName))
                errs.Add("dbName is empty");

            var username = _configuration.GetValue<string>("DBUSERNAME");
            if (string.IsNullOrEmpty(username))
                errs.Add("username is empty");

            if (errs.Count > 0)
                return Task.FromResult(HealthCheckResult.Unhealthy(string.Join(", ", errs)));

            return Task.FromResult(HealthCheckResult.Healthy("Configuration is ok"));
        }

    }
}