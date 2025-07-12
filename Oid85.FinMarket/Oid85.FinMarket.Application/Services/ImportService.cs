using System.Globalization;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services;

public class ImportService(
    IResourceStoreService resourceStoreService,
    IShareMultiplicatorRepository shareMultiplicatorRepository) 
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
        const int revenueCapIndex = 7;
        
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
                Revenue = GetDouble(data[i][revenueCapIndex]),
            };
        }
        
        await shareMultiplicatorRepository.AddOrUpdateAsync(multiplicators);
        
        return;

        double GetDouble(string str)
        {
            string sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            str = str
                .Replace("%", "")
                .Replace(".", sep)
                .Replace(",", sep)
                .Replace(" ", "")
                .Trim();
            
            return string.IsNullOrEmpty(str) ? 0.0 : double.Parse(str);
        }
    }
    
    private async Task ImportBanksMultiplicatorsAsync()
    {
        var data = await resourceStoreService.GetCsvAsync(KnownCsvPathes.BanksMultiplicators);
    }
}