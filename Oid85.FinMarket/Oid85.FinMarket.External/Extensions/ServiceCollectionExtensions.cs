using Microsoft.Extensions.DependencyInjection;
using Oid85.FinMarket.External.LiteDb;
using Oid85.FinMarket.External.Postgres;
using Oid85.FinMarket.External.Tinkoff;

namespace Oid85.FinMarket.External.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddExternalServices(this IServiceCollection services)
        {
            services.AddTransient<IPostgresService, PostgresService>();
            services.AddTransient<ILiteDbService, LiteDbService>();
            services.AddTransient<ITinkoffService, TinkoffService>();
        }
    }
}
