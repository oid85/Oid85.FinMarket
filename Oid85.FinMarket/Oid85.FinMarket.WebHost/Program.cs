using Oid85.FinMarket.WebHost.Extensions;
using Oid85.FinMarket.WebHost.HostedServices;
using Oid85.FinMarket.External.Extensions;

namespace Oid85.FinMarket.WebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddMemoryCache();

            builder.Services.ConfigureLogger();
            builder.Services.ConfigureSwagger(builder.Configuration);
            builder.Services.ConfigureCors(builder.Configuration);            
            builder.Services.ConfigureExternalServices();
            builder.Services.ConfigureQuartz(builder.Configuration);

            builder.Services.AddHostedService<InitHostedService>();

            var app = builder.Build();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1");
            });

            app.MapControllers();

            app.Run();
        }
    }
}
