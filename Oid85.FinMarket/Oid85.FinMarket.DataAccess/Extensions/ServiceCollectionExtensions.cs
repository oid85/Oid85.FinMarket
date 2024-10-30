using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Interceptors;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.DataAccess.Repositories;

namespace Oid85.FinMarket.DataAccess.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureFinMarketDataAccess(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
                
            services.AddDbContext<FinMarketContext>((serviceProvider, options) =>
            {
                var updateInterceptor = serviceProvider.GetRequiredService<UpdateAuditableEntitiesInterceptor>();
                
                options
                    .UseNpgsql(configuration.GetValue<string>(KnownSettingsKeys.Postgres_ConnectionString))
                    .AddInterceptors(updateInterceptor);
            }, ServiceLifetime.Scoped, ServiceLifetime.Scoped);

            services.AddAutoMapper(typeof(FinMarketMappingProfile));
            
            services.AddTransient<IShareRepository, ShareRepository>();
            services.AddTransient<IBondRepository, BondRepository>();
            services.AddTransient<IDividendInfoRepository, DividendInfoRepository>();
            services.AddTransient<IAnalyseResultRepository, AnalyseResultRepository>();
            services.AddTransient<ICandleRepository, CandleRepository>();
        }
    }
}
