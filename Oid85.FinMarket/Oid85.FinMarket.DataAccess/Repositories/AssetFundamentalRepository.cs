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
        var entity = new AssetFundamentalEntity
        {
            Date = model.Date,
            InstrumentId = model.InstrumentId,
            Currency = model.Currency,
            MarketCapitalization = model.MarketCapitalization,
            HighPriceLast52Weeks = model.HighPriceLast52Weeks,
            LowPriceLast52Weeks = model.LowPriceLast52Weeks,
            AverageDailyVolumeLast10Days = model.AverageDailyVolumeLast10Days,
            AverageDailyVolumeLast4Weeks = model.AverageDailyVolumeLast4Weeks,
            Beta = model.Beta,
            FreeFloat = model.FreeFloat,
            ForwardAnnualDividendYield = model.ForwardAnnualDividendYield,
            SharesOutstanding = model.SharesOutstanding,
            RevenueTtm = model.RevenueTtm,
            EbitdaTtm = model.EbitdaTtm,
            NetIncomeTtm = model.NetIncomeTtm,
            EpsTtm = model.EpsTtm,
            DilutedEpsTtm = model.DilutedEpsTtm,
            FreeCashFlowTtm = model.FreeCashFlowTtm,
            FiveYearAnnualRevenueGrowthRate = model.FiveYearAnnualRevenueGrowthRate,
            ThreeYearAnnualRevenueGrowthRate = model.ThreeYearAnnualRevenueGrowthRate,
            PeRatioTtm = model.PeRatioTtm,
            PriceToSalesTtm = model.PriceToSalesTtm,
            PriceToBookTtm = model.PriceToBookTtm,
            PriceToFreeCashFlowTtm = model.PriceToFreeCashFlowTtm,
            TotalEnterpriseValueMrq = model.TotalEnterpriseValueMrq,
            EvToEbitdaMrq = model.EvToEbitdaMrq,
            NetMarginMrq = model.NetMarginMrq,
            NetInterestMarginMrq = model.NetInterestMarginMrq,
            Roe = model.Roe,
            Roa = model.Roa,
            Roic = model.Roic,
            TotalDebtMrq = model.TotalDebtMrq,
            TotalDebtToEquityMrq = model.TotalDebtToEquityMrq,
            TotalDebtToEbitdaMrq = model.TotalDebtToEbitdaMrq,
            FreeCashFlowToPrice = model.FreeCashFlowToPrice,
            NetDebtToEbitda = model.NetDebtToEbitda,
            CurrentRatioMrq = model.CurrentRatioMrq,
            FixedChargeCoverageRatioFy = model.FixedChargeCoverageRatioFy,
            DividendYieldDailyTtm = model.DividendYieldDailyTtm,
            DividendRateTtm = model.DividendRateTtm,
            DividendsPerShare = model.DividendsPerShare,
            FiveYearsAverageDividendYield = model.FiveYearsAverageDividendYield,
            FiveYearAnnualDividendGrowthRate = model.FiveYearAnnualDividendGrowthRate,
            DividendPayoutRatioFy = model.DividendPayoutRatioFy,
            BuyBackTtm = model.BuyBackTtm,
            OneYearAnnualRevenueGrowthRate = model.OneYearAnnualRevenueGrowthRate,
            DomicileIndicatorCode = model.DomicileIndicatorCode,
            AdrToCommonShareRatio = model.AdrToCommonShareRatio,
            NumberOfEmployees = model.NumberOfEmployees,
            ExDividendDate = model.ExDividendDate,
            FiscalPeriodStartDate = model.FiscalPeriodStartDate,
            FiscalPeriodEndDate = model.FiscalPeriodEndDate,
            RevenueChangeFiveYears = model.RevenueChangeFiveYears,
            EpsChangeFiveYears = model.EpsChangeFiveYears,
            EbitdaChangeFiveYears = model.EbitdaChangeFiveYears,
            TotalDebtChangeFiveYears = model.TotalDebtChangeFiveYears,
            EvToSales = model.EvToSales
        };

        return entity;
    }
    
    private AssetFundamental GetModel(AssetFundamentalEntity entity)
    {
        var model = new AssetFundamental
        {
            Id = entity.Id,
            Date = entity.Date,
            InstrumentId = entity.InstrumentId,
            Currency = entity.Currency,
            MarketCapitalization = entity.MarketCapitalization,
            HighPriceLast52Weeks = entity.HighPriceLast52Weeks,
            LowPriceLast52Weeks = entity.LowPriceLast52Weeks,
            AverageDailyVolumeLast10Days = entity.AverageDailyVolumeLast10Days,
            AverageDailyVolumeLast4Weeks = entity.AverageDailyVolumeLast4Weeks,
            Beta = entity.Beta,
            FreeFloat = entity.FreeFloat,
            ForwardAnnualDividendYield = entity.ForwardAnnualDividendYield,
            SharesOutstanding = entity.SharesOutstanding,
            RevenueTtm = entity.RevenueTtm,
            EbitdaTtm = entity.EbitdaTtm,
            NetIncomeTtm = entity.NetIncomeTtm,
            EpsTtm = entity.EpsTtm,
            DilutedEpsTtm = entity.DilutedEpsTtm,
            FreeCashFlowTtm = entity.FreeCashFlowTtm,
            FiveYearAnnualRevenueGrowthRate = entity.FiveYearAnnualRevenueGrowthRate,
            ThreeYearAnnualRevenueGrowthRate = entity.ThreeYearAnnualRevenueGrowthRate,
            PeRatioTtm = entity.PeRatioTtm,
            PriceToSalesTtm = entity.PriceToSalesTtm,
            PriceToBookTtm = entity.PriceToBookTtm,
            PriceToFreeCashFlowTtm = entity.PriceToFreeCashFlowTtm,
            TotalEnterpriseValueMrq = entity.TotalEnterpriseValueMrq,
            EvToEbitdaMrq = entity.EvToEbitdaMrq,
            NetMarginMrq = entity.NetMarginMrq,
            NetInterestMarginMrq = entity.NetInterestMarginMrq,
            Roe = entity.Roe,
            Roa = entity.Roa,
            Roic = entity.Roic,
            TotalDebtMrq = entity.TotalDebtMrq,
            TotalDebtToEquityMrq = entity.TotalDebtToEquityMrq,
            TotalDebtToEbitdaMrq = entity.TotalDebtToEbitdaMrq,
            FreeCashFlowToPrice = entity.FreeCashFlowToPrice,
            NetDebtToEbitda = entity.NetDebtToEbitda,
            CurrentRatioMrq = entity.CurrentRatioMrq,
            FixedChargeCoverageRatioFy = entity.FixedChargeCoverageRatioFy,
            DividendYieldDailyTtm = entity.DividendYieldDailyTtm,
            DividendRateTtm = entity.DividendRateTtm,
            DividendsPerShare = entity.DividendsPerShare,
            FiveYearsAverageDividendYield = entity.FiveYearsAverageDividendYield,
            FiveYearAnnualDividendGrowthRate = entity.FiveYearAnnualDividendGrowthRate,
            DividendPayoutRatioFy = entity.DividendPayoutRatioFy,
            BuyBackTtm = entity.BuyBackTtm,
            OneYearAnnualRevenueGrowthRate = entity.OneYearAnnualRevenueGrowthRate,
            DomicileIndicatorCode = entity.DomicileIndicatorCode,
            AdrToCommonShareRatio = entity.AdrToCommonShareRatio,
            NumberOfEmployees = entity.NumberOfEmployees,
            ExDividendDate = entity.ExDividendDate,
            FiscalPeriodStartDate = entity.FiscalPeriodStartDate,
            FiscalPeriodEndDate = entity.FiscalPeriodEndDate,
            RevenueChangeFiveYears = entity.RevenueChangeFiveYears,
            EpsChangeFiveYears = entity.EpsChangeFiveYears,
            EbitdaChangeFiveYears = entity.EbitdaChangeFiveYears,
            TotalDebtChangeFiveYears = entity.TotalDebtChangeFiveYears,
            EvToSales = entity.EvToSales
        };

        return model;
    }
}
