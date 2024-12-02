using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Logging.DataAccess.Entities;
using Oid85.FinMarket.Logging.DataAccess.Schemas;
using Oid85.FinMarket.Logging.KnownConstants;

namespace Oid85.FinMarket.Logging.DataAccess;

public class LogContext(DbContextOptions<LogContext> options) : DbContext(options)
{
    public DbSet<LogRecordEntity> LogEntities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
    {
        optionBuilder.UseSnakeCaseNamingConvention();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .HasDefaultSchema(KnownDatabaseSchemas.Log)
            .ApplyConfigurationsFromAssembly(
                typeof(LogContext).Assembly,
                type => type
                    .GetInterface(typeof(ILogSchema).ToString()) != null)
            .UseIdentityAlwaysColumns();
    }    
}