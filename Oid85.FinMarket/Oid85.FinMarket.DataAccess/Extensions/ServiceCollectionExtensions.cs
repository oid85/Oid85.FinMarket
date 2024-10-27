using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Oid85.FinMarket.DataAccess.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureFinMarketDataAccess(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<FinMarketContext>(options =>
            {
                options.UseNpgsql(configuration.GetValue<string>("Postgres:ConnectionString"));
            }, ServiceLifetime.Scoped, ServiceLifetime.Scoped);
        }
    }
}
