using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UnsDashboard.Server.Model
{
    public class DatabaseContext : DbContext
    {
        private string ConnectionString { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Server> Servers { get; set; }

        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DatabaseContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public static DbContextOptionsBuilder GetOptions(
            string connectionString,
            DbContextOptionsBuilder builder)
        {
            return builder.UseSqlite(connectionString);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (ConnectionString != null)
            {
                GetOptions(ConnectionString, optionsBuilder);
            }
        }
    }
}
