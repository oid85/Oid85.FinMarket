using Oid85.FinMarket.Storage.WebHost.Converters;
using Oid85.FinMarket.Storage.WebHost.Helpers;
using Oid85.FinMarket.Storage.WebHost.HostedServices;
using Oid85.FinMarket.Storage.WebHost.Repositories;
using Oid85.FinMarket.Storage.WebHost.Services;

namespace Oid85.FinMarket.Storage.WebHost.ConfigureServices
{
    public class ConfigureServicesDependencyInjection
    {
        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ModelConverter>();
            services.AddTransient<ValidateHelper>();
            
            services.AddTransient<StockRepository>();
            services.AddTransient<CandleRepository>();
            
            services.AddTransient<DownloadCandlesService>();

            services.AddHostedService<InitHostedService>();
            // services.AddHostedService<_1D_DownloadCandlesHostedService>();
        }
    }
}