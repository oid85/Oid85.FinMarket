using System.Text.Json;
using NLog;
using Oid85.FinMarket.Logging.DataAccess.Repositories;
using Oid85.FinMarket.Logging.KnownConstants;
using Oid85.FinMarket.Logging.Models;

namespace Oid85.FinMarket.Logging.Services
{
    /// <inheritdoc />
    public class LogService(
        ILogRepository logRepository,
        ILogger logger) : ILogService
    {
        public async Task LogTrace(string message)
        {
            logger.Trace(message);
            
            var logRecord = new LogRecord
            {
                Date = DateTime.UtcNow,
                Level = KnownLogLevels.Trace,
                Message = message
            };
            
            await logRepository.AddAsync(logRecord);
        }

        public async Task LogInfo(string message)
        {
            logger.Info(message);
            
            var logRecord = new LogRecord
            {
                Date = DateTime.UtcNow,
                Level = KnownLogLevels.Info,
                Message = message
            };
            
            await logRepository.AddAsync(logRecord);
        }

        public async Task LogError(string message)
        {
            logger.Error(message);
            
            var logRecord = new LogRecord
            {
                Date = DateTime.UtcNow,
                Level = KnownLogLevels.Error,
                Message = message
            };
            
            await logRepository.AddAsync(logRecord);
        }

        public async Task LogException(Exception exception)
        {
            logger.Error(exception);

            var parameters = new Dictionary<string, string>()
            {
                { "Source", exception.Source ?? string.Empty },
                { "HelpLink", exception.HelpLink ?? string.Empty },
                { "StackTrace", exception.StackTrace ?? string.Empty },
            };
            
            var logRecord = new LogRecord
            {
                Date = DateTime.UtcNow,
                Level = KnownLogLevels.Error,
                Message = exception.Message,
                Parameters = JsonSerializer.Serialize(parameters)
            };
            
            await logRepository.AddAsync(logRecord);
        }
    }
}
