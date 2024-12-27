using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IAssetFundamentalRepository
{
    Task AddOrUpdateAsync(List<AssetFundamental> assetFundamentals);
    Task<List<AssetFundamental>> GetAsync(Guid instrumentId);
    Task<AssetFundamental?> GetLastAsync(Guid instrumentId);
}