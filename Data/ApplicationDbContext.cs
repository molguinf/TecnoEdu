using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TecnoEdu.Models;

namespace TecnoEdu.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Institute> Institutes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Institute>().HasIndex(institute => institute.OwnerId).IsUnique();
            builder.Entity<Institute>().HasOne<IdentityUser>().WithOne()
                .HasForeignKey<Institute>(institute => institute.OwnerId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
