using Newtonsoft.Json.Linq;
using Oid85.FinMarket.Configuration.Common;
using Oid85.FinMarket.Configuration.Data;

namespace Oid85.FinMarket.Storage.WebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var appPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

            string json = File.ReadAllText(appPath);
            var data = JObject.Parse(json);
            string? path = (string?)data[DataBaseNames.SettingsDataBaseName];

            if (string.IsNullOrEmpty(path))
                throw new NullReferenceException(nameof(path));

            var hostBuilder = Host.CreateDefaultBuilder(args);

            hostBuilder.ConfigureAppConfiguration(builder =>
            {
                builder.AddJsonFile(appPath);
                builder.AddDataBase(path);
            });

            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseUrls($"http://0.0.0.0:{Ports.StoragePort}");
            });

            hostBuilder.UseWindowsService();

            return hostBuilder;
        }
    }
}