using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oid85.FinMarket.Configuration.Common;

namespace Oid85.FinMarket.DAL.ConfigureServices
{
    public class ConfigureServicesDAL
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetValue<string>(ConfigParameterNames.ConnectionStringStorage)!;
            bool debugMode = configuration.GetValue<bool>(ConfigDebugParameterNames.DebugMode);
            
            services.AddDbContext<StorageDataBaseContext>(options =>
            {
                options.UseNpgsql(connectionString);

                if (debugMode)
                {
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                }
            }, ServiceLifetime.Scoped);
        }
    }
}
