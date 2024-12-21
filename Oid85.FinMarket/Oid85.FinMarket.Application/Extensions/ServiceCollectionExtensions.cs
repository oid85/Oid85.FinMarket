using Microsoft.Extensions.DependencyInjection;
using Oid85.FinMarket.Application.HostedService;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Services;

namespace Oid85.FinMarket.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<ILoadService, LoadService>();
        services.AddTransient<IAnalyseService, AnalyseService>();
        services.AddTransient<IReportService, ReportService>();
        services.AddTransient<IJobService, JobService>();
            
        services.AddHostedService<RegisterHangfireJobs>();
    }
}