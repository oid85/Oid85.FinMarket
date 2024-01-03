using NLog;
using NLog.Web;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.Storage.WebHost.ConfigureServices
{
    public class ConfigureServicesLogger
    {
        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            string nlogConfigFile = "nlog.config";

            services.AddTransient(typeof(ILogger), factory =>
            {
                Logger logger = NLogBuilder
                    .ConfigureNLog(nlogConfigFile)
                    .GetLogger(AppDomain.CurrentDomain.FriendlyName);

                return logger;
            });
        }
    }
}