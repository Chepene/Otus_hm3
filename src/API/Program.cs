using API;
using Core.DB;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using HealthChecks.UI.Client;
using API.HealthChecks;
using Persistnce;
using MediatR;
using Application.Users;
using Application.Core;
using Prometheus;
using Prometheus.SystemMetrics;
using Microsoft.Extensions.Options;

internal class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        //builder.Configuration
        //.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //.AddEnvironmentVariables();        

        builder.Services.AddHealthChecks()
            .AddCheck<ConfigurationHealthCheck>("configuration", null, new[] { "startup" })
            .AddCheck<DbHealthCheck>("db", null, new[] { "startup" })
            .AddCheck<DumbHealthCheck>("dumb", null, new[] {"dumb"})
            .ForwardToPrometheus();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<DataContext>(opt =>
        {
            var connectionString = ConfigurationHelper.GetConnectionString(builder.Configuration);
            opt.UseNpgsql(connectionString);
        });

        builder.Services.AddMediatR(typeof(List.Handler).Assembly);

        builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

        builder.Services.AddSystemMetrics();

        builder.Services.AddHttpClient(Options.DefaultName).UseHttpClientMetrics();

        var app = builder.Build();
 
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseHttpMetrics();
        app.UseAuthorization(); 
        app.UseEndpoints(endpoints => 
        {
            endpoints.MapMetrics();

            endpoints.MapHealthChecks("/health-details", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            endpoints.MapHealthChecks("/healthz", new HealthCheckOptions()
            {
                Predicate = (a => a.Tags.Contains("dumb"))
            });

            endpoints.MapHealthChecks("/ready", new HealthCheckOptions()
            {
                Predicate = (a => a.Tags.Contains("dumb"))
            });

            endpoints.MapHealthChecks("/health/startup", new HealthCheckOptions()
            {
                Predicate = (a => a.Tags.Contains("startup"))
            });
        });

        app.MapControllers();

        app.Run();        
    }
    

}