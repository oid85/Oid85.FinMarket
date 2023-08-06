using Microsoft.Extensions.DependencyInjection;

namespace DaGroup.WPAnalyst.DataLake.DAL.ConfigureServices
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFactory<TService, TImplementation>(this IServiceCollection services) 
            where TService : class
            where TImplementation : class, TService
        {
            services.AddTransient<TService, TImplementation>();
            services.AddSingleton<Func<TService>>(x => x.GetService<TService>);
        }
    }
}
