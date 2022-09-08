using Core.DB;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace API.HealthChecks
{
    public class DbHealthCheck : IHealthCheck
    {
       private readonly IConfiguration _configuration;

       public DbHealthCheck(IConfiguration configuration)
       {
            _configuration = configuration;        
       }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (_configuration == null)
                return await Task.FromResult(HealthCheckResult.Unhealthy("config is null"));

            try
            {
                var dbConnectionString = ConfigurationHelper.GetConnectionString(_configuration);
                
                using (var connection = new NpgsqlConnection(dbConnectionString))
                {
                        connection.Open();

                        using (var command = new NpgsqlCommand("select 1", connection))
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                        connection.Close();

                        return await Task.FromResult(HealthCheckResult.Healthy("db is ok"));
                }
            }
            catch(Exception ex)
            {
                return await Task.FromResult(HealthCheckResult.Unhealthy(ex.Message));;
            }           
        }
    }
}