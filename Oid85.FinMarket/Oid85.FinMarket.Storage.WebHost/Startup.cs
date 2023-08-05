using Hangfire;
using Oid85.FinMarket.DAL.ConfigureServices;
using Oid85.FinMarket.Models.ConfigureServices;
using Oid85.FinMarket.Storage.WebHost.ConfigureServices;

namespace Oid85.FinMarket.Storage.WebHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMemoryCache();

            ConfigureServicesDAL.ConfigureServices(services, Configuration);
            ConfigureServicesModels.ConfigureServices(services, Configuration);
            ConfigureServicesDependencyInjection.ConfigureServices(services, Configuration);
            ConfigureServicesLogger.ConfigureServices(services, Configuration);
            ConfigureServicesInvestApi.ConfigureServices(services, Configuration);
            ConfigureServicesHangFire.ConfigureServices(services, Configuration);
            ConfigureServicesSwagger.ConfigureServices(services, Configuration);
            ConfigureServicesCors.ConfigureServices(services, Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("CorsPolicy");
            
            app.UseHangfireDashboard("/dashboard");
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}