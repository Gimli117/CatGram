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
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CatProfile>()
                .HasOne(c => c.ApplicationUser)
                .WithMany(a => a.CatProfiles)
                .HasForeignKey(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CatProfile>()
                .HasMany(c => c.Posts)
                .WithOne(p => p.CatProfile)
                .HasForeignKey(p => p.CatProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}