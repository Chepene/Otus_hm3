using Microsoft.EntityFrameworkCore;
using Core.DB;
using Persistnce;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Start Migrating..");
        
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<DataContext>(opt =>
        {
            var connectionString = ConfigurationHelper.GetConnectionString(builder.Configuration);
            opt.UseNpgsql(connectionString);
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<DataContext>();

            context.Database.EnsureCreated();
        }

        Console.WriteLine("Finished Migration");
        
    }
}
