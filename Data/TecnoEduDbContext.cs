using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TecnoEdu.Models.Entidades;

namespace TecnoEdu.Data;

// Un único DbContext permite crear correctamente todas las FK del diagrama.
public class TecnoEduDbContext : IdentityDbContext<ApplicationUser>
{
    public TecnoEduDbContext(DbContextOptions<TecnoEduDbContext> options)
        : base(options) { }

    public DbSet<Instituto> Institutos => Set<Instituto>();
    public DbSet<InstitutoUsuario> InstitutoUsuarios => Set<InstitutoUsuario>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<OfertaCurso> OfertasCurso => Set<OfertaCurso>();
    public DbSet<Reserva> Reservas => Set<Reserva>();
    public DbSet<Consulta> Consultas => Set<Consulta>();
    public DbSet<Mensaje> Mensajes => Set<Mensaje>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);  // Tablas AspNetUsers/AspNetRoles/AspNetUserRoles, etc.
        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.NombreCompleto).HasMaxLength(150).IsRequired();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TecnoEduDbContext).Assembly);
    }
}
