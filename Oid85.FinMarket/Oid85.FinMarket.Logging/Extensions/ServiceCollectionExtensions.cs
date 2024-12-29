using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Logging.DataAccess;
using Oid85.FinMarket.Logging.DataAccess.Repositories;
using Oid85.FinMarket.Logging.Services;

namespace Oid85.FinMarket.Logging.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureLogs(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContextPool<LogContext>((_, options) =>
        {
            options
                .UseNpgsql(configuration
                    .GetValue<string>(KnownSettingsKeys.PostgresFinMarketConnectionString));
        });
            
        services.AddTransient<ILogRepository, LogRepository>();
        services.AddTransient<ILogService, LogService>();
    }

    public static async Task ApplyLogMigrations(this IHost host)
    {
        var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
        await using var scope = scopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<LogContext>();
        await context.Database.MigrateAsync();
    }
}