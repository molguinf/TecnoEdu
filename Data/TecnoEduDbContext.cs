using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TecnoEdu.Models.Entities;

namespace TecnoEdu.Data;

// Un único DbContext permite crear correctamente todas las FK del diagrama.
public class TecnoEduDbContext : IdentityDbContext<ApplicationUser>
{
    public TecnoEduDbContext(DbContextOptions<TecnoEduDbContext> options)
        : base(options) { }

    public DbSet<Institute> Institutes => Set<Institute>();
    public DbSet<InstituteUser> InstituteUsers => Set<InstituteUser>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CourseOffering> CourseOfferings => Set<CourseOffering>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Inquiry> Inquiries => Set<Inquiry>();
    public DbSet<InquiryMessage> Messages => Set<InquiryMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);  // Tablas AspNetUsers/AspNetRoles/AspNetUserRoles, etc.
        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.FullName).HasMaxLength(150).IsRequired();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TecnoEduDbContext).Assembly);
    }
}
