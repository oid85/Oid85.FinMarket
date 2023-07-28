using Microsoft.OpenApi.Models;

namespace Oid85.FinMarket.Storage.WebHost.ConfigureServices
{
    public class ConfigureServicesSwagger
    {
        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
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

        private static string GetXmlCommentsPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SwaggerTest.XML");
        }
    }
}
