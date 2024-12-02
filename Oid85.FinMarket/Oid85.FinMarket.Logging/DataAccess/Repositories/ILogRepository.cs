using Oid85.FinMarket.Logging.Models;

namespace Oid85.FinMarket.Logging.DataAccess.Repositories;

public interface ILogRepository
{
    Task AddAsync(LogRecord logRecord);
}