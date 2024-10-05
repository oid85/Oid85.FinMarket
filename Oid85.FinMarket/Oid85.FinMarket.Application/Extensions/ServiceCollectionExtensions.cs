using Microsoft.Extensions.DependencyInjection;
using Oid85.FinMarket.Application.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.Catalogs;
using Oid85.FinMarket.External.Helpers;
using Oid85.FinMarket.External.Settings;
using Oid85.FinMarket.External.Storage;
using Oid85.FinMarket.External.Tinkoff;

namespace Oid85.FinMarket.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {            
            services.AddTransient<IAnalyseService, AnalyseService>();
        }
    }
}
