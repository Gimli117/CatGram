using CatGram.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CatGram.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
        public DbSet<CatProfile> CatProfiles { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CatProfile>()
                .HasOne(c => c.ApplicationUser)
                .WithMany(a => a.CatProfiles)
                .HasForeignKey(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}