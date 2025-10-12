using Oid85.FinMarket.Domain.Models.Analyse;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IAssetReportEventRepository
{
    Task AddAsync(List<AssetReportEvent> results);
    Task<List<AssetReportEvent>> GetAllAsync();
    Task<List<AssetReportEvent>> GetAsync(List<Guid> instrumentIds);
}