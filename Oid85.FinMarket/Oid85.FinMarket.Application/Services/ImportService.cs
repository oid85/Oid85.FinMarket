using System.Globalization;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services;

public class ImportService(
    IResourceStoreService resourceStoreService,
    IShareMultiplicatorRepository shareMultiplicatorRepository,
    IBankMultiplicatorRepository bankMultiplicatorRepository) 
    : IImportService
{
    public async Task ImportMultiplicatorsAsync()
    {
        await ImportSharesMultiplicatorsAsync();
        await ImportBanksMultiplicatorsAsync();
    }
    
    private async Task ImportSharesMultiplicatorsAsync()
    {
        const int nameIndex = 1;
        const int tickerIndex = 2;
        const int marketCapIndex = 5;
        const int evCapIndex = 6;
        const int revenueIndex = 7;
        const int netIncomeIndex = 8;
        const int ddAoIndex = 9;
        const int ddApIndex = 10;
        const int ddNetIncomeIndex = 11;
        const int peIndex = 12;
        const int psIndex = 13;
        const int pbIndex = 14;
        const int evEbitdaIndex = 15;
        const int ebitdaMarginIndex = 16;
        const int netDebtEbitdaIndex = 17;
        
        var data = await resourceStoreService.GetCsvAsync(KnownCsvPathes.StockMultiplicators);

        var multiplicators = new List<ShareMultiplicator>();
        
        for (int i = 1; i < data.Count; i++)
        {
            var multiplicator = new ShareMultiplicator
            {
                Name = data[i][nameIndex].Trim().ToUpper(),
                Ticker = data[i][tickerIndex].Trim().ToUpper(),
                MarketCap = GetDouble(data[i][marketCapIndex]),
                Ev = GetDouble(data[i][evCapIndex]),
                Revenue = GetDouble(data[i][revenueIndex]),
                NetIncome = GetDouble(data[i][netIncomeIndex]),
                DdAo = GetDouble(data[i][ddAoIndex]),
                DdAp = GetDouble(data[i][ddApIndex]),
                DdNetIncome = GetDouble(data[i][ddNetIncomeIndex]),
                Pe = GetDouble(data[i][peIndex]),
                Ps = GetDouble(data[i][psIndex]),
                Pb = GetDouble(data[i][pbIndex]),
                EvEbitda = GetDouble(data[i][evEbitdaIndex]),
                EbitdaMargin = GetDouble(data[i][ebitdaMarginIndex]),
                NetDebtEbitda = GetDouble(data[i][netDebtEbitdaIndex])
            };
            
            multiplicators.Add(multiplicator);
        }
        
        await shareMultiplicatorRepository.AddOrUpdateAsync(multiplicators);
    }
    
    private async Task ImportBanksMultiplicatorsAsync()
    {
        const int nameIndex = 1;
        const int tickerIndex = 2;
        const int marketCapIndex = 5;
        const int netOperatingIncomeIndex = 6;
        const int netIncomeIndex = 7;
        const int ddAoIndex = 8;
        const int ddApIndex = 9;
        const int ddNetIncomeIndex = 10;
        const int peIndex = 11;
        const int pbIndex = 12;
        const int netInterestMarginIndex = 13;
        const int roeIndex = 14;
        const int roaIndex = 15;
        
        var data = await resourceStoreService.GetCsvAsync(KnownCsvPathes.BankMultiplicators);

        var multiplicators = new List<BankMultiplicator>();
        
        for (int i = 1; i < data.Count; i++)
        {
            var multiplicator = new BankMultiplicator
            {
                Name = data[i][nameIndex].Trim().ToUpper(),
                Ticker = data[i][tickerIndex].Trim().ToUpper(),
                MarketCap = GetDouble(data[i][marketCapIndex]),
                NetOperatingIncome = GetDouble(data[i][netOperatingIncomeIndex]),
                NetIncome = GetDouble(data[i][netIncomeIndex]),
                DdAo = GetDouble(data[i][ddAoIndex]),
                DdAp = GetDouble(data[i][ddApIndex]),
                DdNetIncome = GetDouble(data[i][ddNetIncomeIndex]),
                Pe = GetDouble(data[i][peIndex]),
                Pb = GetDouble(data[i][pbIndex]),
                NetInterestMargin = GetDouble(data[i][netInterestMarginIndex]),
                Roe = GetDouble(data[i][roeIndex]),
                Roa = GetDouble(data[i][roaIndex])
            };
            
            multiplicators.Add(multiplicator);
        }
        
        await bankMultiplicatorRepository.AddOrUpdateAsync(multiplicators);
    }
    
    private double GetDouble(string str)
    {
        string sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        str = str
            .Replace(" ", "")
            .Replace("%", "")
            .Replace(".", sep)
            .Replace(",", sep)
            .Trim();
            
        return string.IsNullOrEmpty(str) ? 0.0 : double.Parse(str);
    }
}