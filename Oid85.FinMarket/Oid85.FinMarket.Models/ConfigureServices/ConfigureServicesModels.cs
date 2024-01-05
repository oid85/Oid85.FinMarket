using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Oid85.FinMarket.Models.ConfigureServices
{

    public class ConfigureServicesModels
    {
        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<Stock>();
            services.AddTransient<Candle>();
            services.AddTransient<DownloadRequest>();
        }
    }
}
