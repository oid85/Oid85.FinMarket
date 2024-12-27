using Hangfire;
using Oid85.FinMarket.WebHost.Extensions;
using Oid85.FinMarket.External.Extensions;
using Oid85.FinMarket.Application.Extensions;
using Oid85.FinMarket.DataAccess.Extensions;
using Oid85.FinMarket.Logging.Extensions;

namespace Oid85.FinMarket.WebHost;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
            
        builder.Services.AddControllers();
        builder.Services.AddMemoryCache();
        builder.Services.ConfigureLogger();
        builder.Services.ConfigureSwagger(builder.Configuration);
        builder.Services.ConfigureCors(builder.Configuration);            
        builder.Services.ConfigureApplicationServices();
        builder.Services.ConfigureExternalServices(builder.Configuration);
        builder.Services.ConfigureFinMarketDataAccess(builder.Configuration);
        builder.Services.ConfigureLogs(builder.Configuration);
        builder.Services.ConfigureHangfire(builder.Configuration);
            
        builder.Services.AddWindowsService(options =>
        {
            options.ServiceName = "Oid85 FinMarket Service";
        });

        var app = builder.Build();
        
        app.UseRouting();

        app.UseCors("CorsPolicy");

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = "";
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1");
        });

        app.UseHangfireDashboard("/dashboard");
        
        await app.RegisterHangfireJobs(builder.Configuration);
        
        app.MapControllers();

        await app.RunAsync();
    }
}