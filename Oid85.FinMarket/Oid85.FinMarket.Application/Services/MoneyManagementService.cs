using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class MoneyManagementService(
    IFutureRepository futureRepository,
    IShareRepository shareRepository,
    IResourceStoreService resourceStoreService) 
    : IMoneyManagementService
{
    /// <inheritdoc />
    public async Task<int> GetPositionSizeAsync(string ticker, double orderPrice, double money)
    {
        if (money <= orderPrice)
            return 0;
        
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        bool isFuture = await IsFuture(ticker);

        if (isFuture)
        {
            var future = await futureRepository.GetAsync(ticker);
            return future is null || orderPrice == 0.0 || future.BasicAssetSize == 0.0 ? 
                0 : Convert.ToInt32(money / (orderPrice * future.BasicAssetSize) * algoConfigResource.MoneyManagementResource.FutureLeverage);
        }
        
        var share = await shareRepository.GetAsync(ticker);
        return share is null || orderPrice == 0.0 ? 
            0 : Convert.ToInt32(money / orderPrice * algoConfigResource.MoneyManagementResource.ShareLeverage);
    }

    /// <inheritdoc />
    public async Task<(int First, int Second)> GetPositionSizeAsync(
        (string First, string Second) ticker,
        (double First, double Second) orderPrice,
        (double First, double Second) money)
    {
        if (money.First + money.Second <= orderPrice.First + orderPrice.Second)
            return (0, 0);
        
        var algoConfigResource = await resourceStoreService.GetAlgoConfigAsync();
        bool isFutureFirst= await IsFuture(ticker.First);
        bool isFutureSecond = await IsFuture(ticker.Second);

        (int First, int Second) positionSize = (0, 0);

        if (isFutureFirst)
        {
            var future = await futureRepository.GetAsync(ticker.First);
            positionSize.First = future is null || orderPrice.First == 0.0 || future.BasicAssetSize == 0.0 ? 
                0 : Convert.ToInt32(money.First / (orderPrice.First * future.BasicAssetSize) * algoConfigResource.MoneyManagementResource.StatisticalArbitrageFutureLeverage);
        }

        else
        {
            var share = await shareRepository.GetAsync(ticker.First);
            positionSize.First = share is null || orderPrice.First == 0.0 ? 
                0 : Convert.ToInt32(money.First / orderPrice.First * algoConfigResource.MoneyManagementResource.StatisticalArbitrageShareLeverage);
        }
        
        if (isFutureSecond)
        {
            var future = await futureRepository.GetAsync(ticker.Second);
            positionSize.Second = future is null || orderPrice.Second == 0.0 || future.BasicAssetSize == 0.0 ? 
                0 : Convert.ToInt32(money.Second / (orderPrice.Second * future.BasicAssetSize) * algoConfigResource.MoneyManagementResource.StatisticalArbitrageFutureLeverage);
        }

        else
        {
            var share = await shareRepository.GetAsync(ticker.Second);
            positionSize.Second = share is null || orderPrice.Second == 0.0 ? 
                0 : Convert.ToInt32(money.Second / orderPrice.Second * algoConfigResource.MoneyManagementResource.StatisticalArbitrageShareLeverage);
        }        
        
        if (positionSize.First == 0 || positionSize.Second == 0)
            return (0, 0);
        
        return positionSize;
    }

    /// <summary>
    /// Признак фьючерса
    /// </summary>
    /// <param name="ticker">Тикер инструмента</param>
    private async Task<bool> IsFuture(string ticker) =>
        (await futureRepository.GetAllAsync()).Select(x => x.Ticker).Contains(ticker);
}