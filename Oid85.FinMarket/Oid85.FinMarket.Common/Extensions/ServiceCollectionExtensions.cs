using Microsoft.Extensions.DependencyInjection;
using Oid85.FinMarket.Common.Logger;

namespace Oid85.FinMarket.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureFinMarketLogger(
        this IServiceCollection services)
    {
        services.AddTransient<FinMarketLogger>();
    }
}