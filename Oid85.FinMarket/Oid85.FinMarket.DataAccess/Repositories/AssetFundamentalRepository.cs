using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class AssetFundamentalRepository(
    FinMarketContext context) 
    : IAssetFundamentalRepository
{
    public async Task AddAsync(List<AssetFundamental> assetFundamentals)
    {
        if (assetFundamentals is [])
            return;

        var entities = new List<AssetFundamentalEntity>();
        
        foreach (var assetFundamental in assetFundamentals)
            if (!await context.AssetFundamentalEntities
                    .AnyAsync(x => 
                        x.InstrumentId == assetFundamental.InstrumentId
                        && x.Date == assetFundamental.Date))
                entities.Add(GetEntity(assetFundamental));

        await context.AssetFundamentalEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task<List<AssetFundamental>> GetAsync(Guid instrumentId) =>
        (await context.AssetFundamentalEntities
            .Where(x => instrumentId == x.InstrumentId)
            .OrderBy(x => x.Date)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<AssetFundamental?> GetLastAsync(Guid instrumentId)
    {
        var entity = await context.AssetFundamentalEntities
            .Where(x => x.InstrumentId == instrumentId)
            .OrderByDescending(x => x.Date)
            .Take(1)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (entity is null)
            return null;
        
        var model = GetModel(entity);
        
        return model;
    }
    
    private AssetFundamentalEntity GetEntity(AssetFundamental model)
    {
        var entity = new AssetFundamentalEntity();
        
        entity.Date = model.Date;
        entity.InstrumentId = model.InstrumentId;
        entity.Currency = model.Currency;
        entity.MarketCapitalization = model.MarketCapitalization;
        entity.HighPriceLast52Weeks = model.HighPriceLast52Weeks;
        entity.LowPriceLast52Weeks = model.LowPriceLast52Weeks;
        entity.AverageDailyVolumeLast10Days = model.AverageDailyVolumeLast10Days;
        entity.AverageDailyVolumeLast4Weeks = model.AverageDailyVolumeLast4Weeks;
        entity.Beta = model.Beta;
        entity.FreeFloat = model.FreeFloat;
        entity.ForwardAnnualDividendYield = model.ForwardAnnualDividendYield;
        entity.SharesOutstanding = model.SharesOutstanding;
        entity.RevenueTtm = model.RevenueTtm;
        entity.EbitdaTtm = model.EbitdaTtm;
        entity.NetIncomeTtm = model.NetIncomeTtm;
        entity.EpsTtm = model.EpsTtm;
        entity.DilutedEpsTtm = model.DilutedEpsTtm;
        entity.FreeCashFlowTtm = model.FreeCashFlowTtm;
        entity.FiveYearAnnualRevenueGrowthRate = model.FiveYearAnnualRevenueGrowthRate;
        entity.ThreeYearAnnualRevenueGrowthRate = model.ThreeYearAnnualRevenueGrowthRate;
        entity.PeRatioTtm = model.PeRatioTtm;
        entity.PriceToSalesTtm = model.PriceToSalesTtm;
        entity.PriceToBookTtm = model.PriceToBookTtm;
        entity.PriceToFreeCashFlowTtm = model.PriceToFreeCashFlowTtm;
        entity.TotalEnterpriseValueMrq = model.TotalEnterpriseValueMrq;
        entity.EvToEbitdaMrq = model.EvToEbitdaMrq;
        entity.NetMarginMrq = model.NetMarginMrq;
        entity.NetInterestMarginMrq = model.NetInterestMarginMrq;
        entity.Roe = model.Roe;
        entity.Roa = model.Roa;
        entity.Roic = model.Roic;
        entity.TotalDebtMrq = model.TotalDebtMrq;
        entity.TotalDebtToEquityMrq = model.TotalDebtToEquityMrq;
        entity.TotalDebtToEbitdaMrq = model.TotalDebtToEbitdaMrq;
        entity.FreeCashFlowToPrice = model.FreeCashFlowToPrice;
        entity.NetDebtToEbitda = model.NetDebtToEbitda;
        entity.CurrentRatioMrq = model.CurrentRatioMrq;
        entity.FixedChargeCoverageRatioFy = model.FixedChargeCoverageRatioFy;
        entity.DividendYieldDailyTtm = model.DividendYieldDailyTtm;
        entity.DividendRateTtm = model.DividendRateTtm;
        entity.DividendsPerShare = model.DividendsPerShare;
        entity.FiveYearsAverageDividendYield = model.FiveYearsAverageDividendYield;
        entity.FiveYearAnnualDividendGrowthRate = model.FiveYearAnnualDividendGrowthRate;
        entity.DividendPayoutRatioFy = model.DividendPayoutRatioFy;
        entity.BuyBackTtm = model.BuyBackTtm;
        entity.OneYearAnnualRevenueGrowthRate = model.OneYearAnnualRevenueGrowthRate;
        entity.DomicileIndicatorCode = model.DomicileIndicatorCode;
        entity.AdrToCommonShareRatio = model.AdrToCommonShareRatio;
        entity.NumberOfEmployees = model.NumberOfEmployees;
        entity.ExDividendDate = model.ExDividendDate;
        entity.FiscalPeriodStartDate = model.FiscalPeriodStartDate;
        entity.FiscalPeriodEndDate = model.FiscalPeriodEndDate;
        entity.RevenueChangeFiveYears = model.RevenueChangeFiveYears;
        entity.EpsChangeFiveYears = model.EpsChangeFiveYears;
        entity.EbitdaChangeFiveYears = model.EbitdaChangeFiveYears;
        entity.TotalDebtChangeFiveYears = model.TotalDebtChangeFiveYears;
        entity.EvToSales = model.EvToSales;
        
        return entity;
    }
    
    private AssetFundamental GetModel(AssetFundamentalEntity entity)
    {
        var model = new AssetFundamental();
        
        model.Id = entity.Id;
        model.Date = entity.Date;
        model.InstrumentId = entity.InstrumentId;
        model.Currency = entity.Currency;
        model.MarketCapitalization = entity.MarketCapitalization;
        model.HighPriceLast52Weeks = entity.HighPriceLast52Weeks;
        model.LowPriceLast52Weeks = entity.LowPriceLast52Weeks;
        model.AverageDailyVolumeLast10Days = entity.AverageDailyVolumeLast10Days;
        model.AverageDailyVolumeLast4Weeks = entity.AverageDailyVolumeLast4Weeks;
        model.Beta = entity.Beta;
        model.FreeFloat = entity.FreeFloat;
        model.ForwardAnnualDividendYield = entity.ForwardAnnualDividendYield;
        model.SharesOutstanding = entity.SharesOutstanding;
        model.RevenueTtm = entity.RevenueTtm;
        model.EbitdaTtm = entity.EbitdaTtm;
        model.NetIncomeTtm = entity.NetIncomeTtm;
        model.EpsTtm = entity.EpsTtm;
        model.DilutedEpsTtm = entity.DilutedEpsTtm;
        model.FreeCashFlowTtm = entity.FreeCashFlowTtm;
        model.FiveYearAnnualRevenueGrowthRate = entity.FiveYearAnnualRevenueGrowthRate;
        model.ThreeYearAnnualRevenueGrowthRate = entity.ThreeYearAnnualRevenueGrowthRate;
        model.PeRatioTtm = entity.PeRatioTtm;
        model.PriceToSalesTtm = entity.PriceToSalesTtm;
        model.PriceToBookTtm = entity.PriceToBookTtm;
        model.PriceToFreeCashFlowTtm = entity.PriceToFreeCashFlowTtm;
        model.TotalEnterpriseValueMrq = entity.TotalEnterpriseValueMrq;
        model.EvToEbitdaMrq = entity.EvToEbitdaMrq;
        model.NetMarginMrq = entity.NetMarginMrq;
        model.NetInterestMarginMrq = entity.NetInterestMarginMrq;
        model.Roe = entity.Roe;
        model.Roa = entity.Roa;
        model.Roic = entity.Roic;
        model.TotalDebtMrq = entity.TotalDebtMrq;
        model.TotalDebtToEquityMrq = entity.TotalDebtToEquityMrq;
        model.TotalDebtToEbitdaMrq = entity.TotalDebtToEbitdaMrq;
        model.FreeCashFlowToPrice = entity.FreeCashFlowToPrice;
        model.NetDebtToEbitda = entity.NetDebtToEbitda;
        model.CurrentRatioMrq = entity.CurrentRatioMrq;
        model.FixedChargeCoverageRatioFy = entity.FixedChargeCoverageRatioFy;
        model.DividendYieldDailyTtm = entity.DividendYieldDailyTtm;
        model.DividendRateTtm = entity.DividendRateTtm;
        model.DividendsPerShare = entity.DividendsPerShare;
        model.FiveYearsAverageDividendYield = entity.FiveYearsAverageDividendYield;
        model.FiveYearAnnualDividendGrowthRate = entity.FiveYearAnnualDividendGrowthRate;
        model.DividendPayoutRatioFy = entity.DividendPayoutRatioFy;
        model.BuyBackTtm = entity.BuyBackTtm;
        model.OneYearAnnualRevenueGrowthRate = entity.OneYearAnnualRevenueGrowthRate;
        model.DomicileIndicatorCode = entity.DomicileIndicatorCode;
        model.AdrToCommonShareRatio = entity.AdrToCommonShareRatio;
        model.NumberOfEmployees = entity.NumberOfEmployees;
        model.ExDividendDate = entity.ExDividendDate;
        model.FiscalPeriodStartDate = entity.FiscalPeriodStartDate;
        model.FiscalPeriodEndDate = entity.FiscalPeriodEndDate;
        model.RevenueChangeFiveYears = entity.RevenueChangeFiveYears;
        model.EpsChangeFiveYears = entity.EpsChangeFiveYears;
        model.EbitdaChangeFiveYears = entity.EbitdaChangeFiveYears;
        model.TotalDebtChangeFiveYears = entity.TotalDebtChangeFiveYears;
        model.EvToSales = entity.EvToSales;

        return model;
    }
}
