using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oid85.FinMarket.Configuration.Common;
using Oid85.FinMarket.DAL;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.Storage.WebHost.HostedServices
{
    public class InitHostedService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public InitHostedService(
            ILogger logger,
            IConfiguration configuration,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_logger == null)
                throw new NullReferenceException(nameof(_logger));

            _logger.Info($"Запуск сервиса {AppDomain.CurrentDomain.FriendlyName}, {AppCommon.AppVersion}");

            if (_configuration == null)
                throw new NullReferenceException(nameof(_configuration));

            bool debugMode = _configuration.GetValue<bool>(ConfigDebugParameterNames.DebugMode);

            if (debugMode)
                _logger.Warn($"Cервис {AppDomain.CurrentDomain.FriendlyName} запущен в режиме отладки");

            using (var scope = _scopeFactory.CreateScope())
            {
                var dataBaseContext = scope.ServiceProvider.GetRequiredService<StorageDataBaseContext>();
                
                await dataBaseContext.Database.MigrateAsync(cancellationToken);
            }
            
            _logger.Info($"Конфигурация: {JsonConvert.SerializeObject(_configuration.GetChildren())}");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}