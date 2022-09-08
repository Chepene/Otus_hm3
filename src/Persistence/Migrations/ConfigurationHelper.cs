using Microsoft.Extensions.Configuration;

namespace Core.DB
{
    public static class ConfigurationHelper
    {
        public static string GetConnectionString(IConfiguration config)
        {
            var host = config.GetValue<string>("DBHOSTNAME");
            var port = config.GetValue<string>("DBPORT");
            var dbName = config.GetValue<string>("DBNAME");
            var username = config.GetValue<string>("DBUSERNAME");
            var password = config.GetValue<string>("DBPASSWORD");      

            return $"Host={host}:{port};Database={dbName};Username={username};Password={password}";
        }
    }
}