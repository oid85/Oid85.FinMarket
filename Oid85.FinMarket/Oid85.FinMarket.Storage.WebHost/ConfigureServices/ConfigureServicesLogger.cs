using NLog;
using NLog.Web;
using Oid85.FinMarket.Configuration.Common;
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

            if (environmentVariable == null)
                nlogConfigFile = "nlog.config";

            else if (environmentVariable == EnvironmentVariableNames.Docker)
                nlogConfigFile = "nlog.docker.config";

            else if (environmentVariable == EnvironmentVariableNames.Development)
                nlogConfigFile = "nlog.development.config";

            else if (environmentVariable == EnvironmentVariableNames.Production)
                nlogConfigFile = "nlog.production.config";

            else if (environmentVariable == EnvironmentVariableNames.Test)
                nlogConfigFile = "nlog.test.config";

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