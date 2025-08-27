using Oid85.FinMarket.Application.Interfaces.Services;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class MoneyManagementService : IMoneyManagementService
{
    /// <inheritdoc />
    public async Task<int> GetPositionSizeAsync(string ticker, double orderPrice, double money)
    {
        return 0;
    }
}