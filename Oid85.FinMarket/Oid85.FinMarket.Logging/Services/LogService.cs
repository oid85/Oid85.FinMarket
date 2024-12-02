using NLog;
using Oid85.FinMarket.Logging.DataAccess.Repositories;
using Oid85.FinMarket.Logging.KnownConstants;
using Oid85.FinMarket.Logging.Models;

namespace Oid85.FinMarket.Logging.Services
{
    public class LogService(
        ILogRepository logRepository,
        ILogger logger) : ILogService
    {
        public async Task LogTrace(string message)
        {
            logger.Trace(message);
            
            try
            {
                var logRecord = new LogRecord
                {
                    Date = DateTime.UtcNow,
                    Level = KnownLogLevels.Trace,
                    Message = message
                };
            
                await logRepository.AddAsync(logRecord);
            }
            
            catch (Exception exception)
            {
                logger.Error(exception);
            }
        }

        public async Task LogInfo(string message)
        {
            logger.Info(message);
            
            try
            {
                var logRecord = new LogRecord
                {
                    Date = DateTime.UtcNow,
                    Level = KnownLogLevels.Info,
                    Message = message
                };
            
                await logRepository.AddAsync(logRecord);
            }
            
            catch (Exception exception)
            {
                logger.Error(exception);
            }
        }

        public async Task LogError(string message)
        {
            logger.Error(message);
            
            try
            {
                var logRecord = new LogRecord
                {
                    Date = DateTime.UtcNow,
                    Level = KnownLogLevels.Error,
                    Message = message
                };
            
                await logRepository.AddAsync(logRecord);
            }
            
            catch (Exception exception)
            {
                logger.Error(exception);
            }
        }
    }
}
