using Microsoft.Extensions.DependencyInjection;
using Oid85.FinMarket.Strategies.Algorithms;
using Oid85.FinMarket.Strategies.Models;

namespace Oid85.FinMarket.Strategies.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureStrategies(this IServiceCollection services)
    {
        services.AddKeyedTransient<Strategy, DonchianBreakoutClassicLongDaily>("DonchianBreakoutClassicLongDaily");
    }
}