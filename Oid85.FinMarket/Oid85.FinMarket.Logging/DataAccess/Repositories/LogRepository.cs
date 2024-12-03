using Oid85.FinMarket.Logging.DataAccess.Entities;
using Oid85.FinMarket.Logging.Models;

namespace Oid85.FinMarket.Logging.DataAccess.Repositories;

public class LogRepository(LogContext context) : ILogRepository
{
    public async Task AddAsync(LogRecord logRecord)
    {
        var entity = new LogRecordEntity
        {
            Id = logRecord.Id,
            Date = logRecord.Date,
            Level = logRecord.Level,
            Message = logRecord.Message,
            Parameters = logRecord.Parameters
        };
        
        await context.LogEntities.AddAsync(entity);
        await context.SaveChangesAsync();
    }
}