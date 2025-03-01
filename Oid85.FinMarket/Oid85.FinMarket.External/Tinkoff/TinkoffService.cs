using Oid85.FinMarket.Domain.Models;
using Candle = Oid85.FinMarket.Domain.Models.Candle;
using Share = Oid85.FinMarket.Domain.Models.Share;
using Bond = Oid85.FinMarket.Domain.Models.Bond;
using Currency = Oid85.FinMarket.Domain.Models.Currency;
using Future = Oid85.FinMarket.Domain.Models.Future;

namespace Oid85.FinMarket.External.Tinkoff;

/// <inheritdoc />
public class TinkoffService(
    GetPricesService getPricesService,
    GetInstrumentsService getInstrumentsService,
    GetCandlesService getCandlesService,
    GetDividendInfoService getDividendInfoService,
    GetBondCouponsService getBondCouponsService,
    GetForecastService getForecastService)
    : ITinkoffService
{
    // <inheritdoc />
    public Task<List<Candle>> GetDailyCandlesAsync(
        Guid instrumentId, DateOnly from, DateOnly to) =>
        getCandlesService.GetDailyCandlesAsync(instrumentId, from, to);

    /// <inheritdoc />
    public Task<List<Candle>> GetDailyCandlesAsync(Guid instrumentId, int year) =>
        getCandlesService.GetDailyCandlesAsync(instrumentId, year);

    /// <inheritdoc />
    public Task<List<FiveMinuteCandle>> GetFiveMinuteCandlesAsync(
        Guid instrumentId, DateTime from, DateTime to) =>
        getCandlesService.GetFiveMinuteCandlesAsync(instrumentId, from, to);

    /// <inheritdoc />
    public Task<List<double>> GetPricesAsync(List<Guid> instrumentIds) =>
        getPricesService.GetPricesAsync(instrumentIds);
    
    /// <inheritdoc />
    public Task<List<Share>> GetSharesAsync() =>
        getInstrumentsService.GetSharesAsync();
    
    /// <inheritdoc />
    public Task<List<Future>> GetFuturesAsync() =>
        getInstrumentsService.GetFuturesAsync();
    
    /// <inheritdoc />
    public Task<List<Bond>> GetBondsAsync() =>
        getInstrumentsService.GetBondsAsync();

    /// <inheritdoc />
    public Task<List<FinIndex>> GetIndexesAsync() =>
        getInstrumentsService.GetIndexesAsync();

    /// <inheritdoc />
    public Task<List<Currency>> GetCurrenciesAsync() =>
        getInstrumentsService.GetCurrenciesAsync();

    /// <inheritdoc />
    public Task<List<DividendInfo>> GetDividendInfoAsync(List<Share> shares) =>
        getDividendInfoService.GetDividendInfoAsync(shares);

    /// <inheritdoc />
    public Task<List<BondCoupon>> GetBondCouponsAsync(List<Bond> bonds) =>
        getBondCouponsService.GetBondCouponsAsync(bonds);

    /// <inheritdoc />
    public Task<(List<ForecastTarget>, ForecastConsensus)> GetForecastAsync(Guid instrumentId) =>
        getForecastService.GetForecastAsync(instrumentId);
}