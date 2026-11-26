using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TecnoEdu.Data;

public sealed class TecnoEduDbContextFactory
    : IDesignTimeDbContextFactory<TecnoEduDbContext>
{
    public TecnoEduDbContext CreateDbContext(string[] args)
    {
        var connection = Environment.GetEnvironmentVariable(
            "ConnectionStrings__TecnoEduConnection");

        if (string.IsNullOrWhiteSpace(connection))
        {
            throw new InvalidOperationException(
                "Set ConnectionStrings__TecnoEduConnection before running EF Core.");
        }

        var builder = new SqlConnectionStringBuilder(connection);

        if (string.IsNullOrWhiteSpace(builder.InitialCatalog))
        {
            throw new InvalidOperationException(
                "A database name is required.");
        }

        if (string.Equals(
            builder.InitialCatalog,
            "TecnoEduDB",
            StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "The original TecnoEduDB database is protected.");
        }

        var options = new DbContextOptionsBuilder<TecnoEduDbContext>()
            .UseSqlServer(connection)
            .Options;

        return new TecnoEduDbContext(options);
    }
}
