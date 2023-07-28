using Oid85.FinMarket.Configuration.Common;

namespace Oid85.FinMarket.Storage.WebHost.ConfigureServices
{
    public class ConfigureServicesInvestApi
    {
        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            string token = configuration.GetValue<string>(ConfigParameterNames.TinkoffApiToken)!;
            
            services.AddInvestApiClient((_, settings) => settings.AccessToken = token);
        }
    }
}