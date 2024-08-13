using Microsoft.EntityFrameworkCore;

namespace testAPI
{
    public class taskDb : DbContext
    {
        public taskDb(DbContextOptions<taskDb> options)
            : base(options) { }

        public DbSet<task> Tasks => Set<task>();
    }
}
