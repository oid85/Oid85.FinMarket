using System.Collections;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IDividendInfoRepository
{
    Task AddOrUpdateAsync(List<DividendInfo> dividendInfos);
    Task<List<DividendInfo>> GetDividendInfosAsync();
}