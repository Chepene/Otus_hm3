using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistnce
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<User> Users {get;set;}
    }
}