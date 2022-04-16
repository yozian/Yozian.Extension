using Microsoft.EntityFrameworkCore;
using Yozian.Extension.Test.Data.Entities;

namespace Yozian.Extension.Test.Data
{
    public class SouthSeaDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("MyTestDb");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
