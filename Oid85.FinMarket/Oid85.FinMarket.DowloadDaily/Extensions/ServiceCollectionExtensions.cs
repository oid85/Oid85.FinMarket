using Microsoft.OpenApi.Models;
using NLog;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.DowloadDaily.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureLogger(this IServiceCollection services)
        {
            string nlogConfigFile = "nlog.config";            

            LogManager
                .Setup()
                .LoadConfigurationFromFile(nlogConfigFile);

            services.AddTransient(typeof(ILogger), factory =>
            {
                Logger logger = LogManager.GetLogger(AppDomain.CurrentDomain.FriendlyName);

                return logger;
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Api",
                    Description = AppDomain.CurrentDomain.FriendlyName
                });

                c.IncludeXmlComments(GetXmlCommentsPath());
            });
        }

        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.SetIsOriginAllowed(host => true);
                    builder.AllowCredentials();
                });
            });
        }

        private static string GetXmlCommentsPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SwaggerTest.XML");
        }
    }
}
