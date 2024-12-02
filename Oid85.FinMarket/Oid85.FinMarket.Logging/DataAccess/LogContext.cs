using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Logging.DataAccess.Entities;
using Oid85.FinMarket.Logging.DataAccess.Schemas;

namespace Oid85.FinMarket.Logging.DataAccess;

public class LogContext : DbContext
{
    public LogContext(DbContextOptions<LogContext> options) : base(options)
    {

    }
    
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