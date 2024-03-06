using ExpressedRealms.DB.UserProfile.PlayerDBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB
{
    public class ExpressedRealmsDbContext : IdentityDbContext<IdentityUser>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new CharacterConfiguration());
            builder.ApplyConfiguration(new PlayerConfiguration());
        }

        public ExpressedRealmsDbContext(DbContextOptions<ExpressedRealmsDbContext> options)
            : base(options) { }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}
