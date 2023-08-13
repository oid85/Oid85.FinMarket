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
            services.AddTransient<ToolsHelper>();
            services.AddTransient<TranslateModelHelper>();
            services.AddTransient<ValidateHelper>();
            
            services.AddTransient<AssetRepository>();
            services.AddTransient<CandleRepository>();
            
            services.AddTransient<DownloadCandlesService>();

            services.AddHostedService<InitHostedService>();
            services.AddHostedService<_1M_SubscribeCandlesHostedService>();
            services.AddHostedService<_1H_DownloadCandlesHostedService>();
            services.AddHostedService<_1D_DownloadCandlesHostedService>();
        }
    }
}