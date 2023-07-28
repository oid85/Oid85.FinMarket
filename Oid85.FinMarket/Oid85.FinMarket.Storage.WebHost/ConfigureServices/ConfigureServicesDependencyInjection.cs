using Oid85.FinMarket.Storage.WebHost.HostedServices;

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
            services.AddHostedService<InitHostedService>();
        }
    }
}