using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB
{
    public class ExpressedRealmsDbContext:DbContext
    {
        public ExpressedRealmsDbContext(DbContextOptions<ExpressedRealmsDbContext> options) : base(options)
        {

        }

        public DbSet<Character> Characters { get; set; }
    }
}