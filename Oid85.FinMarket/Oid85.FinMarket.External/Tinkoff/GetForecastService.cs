using NLog;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace Oid85.FinMarket.External.Tinkoff;

public class GetForecastService(
    ILogger logger,
    InvestApiClient client)
{
    private const int DelayInMilliseconds = 50;
    
        public async Task<(List<ForecastTarget>, ForecastConsensus)> GetForecastAsync(Guid instrumentId)
    {
            await Task.Delay(DelayInMilliseconds);

            var request = CreateGetForecastRequest(instrumentId);
            var response = await SendGetForecastRequest(request);
            
            if (response is null)
                return ([], new());

            var targets = new List<ForecastTarget>();

            if (response.Targets is not null)
                foreach (var targetItem in response.Targets)
                    if (targetItem is not null)
                    {
                        var target = Map(targetItem);
                        targets.Add(target);   
                    }

            var consensus = response.Consensus is null 
                ? new() 
                : Map(response.Consensus);
            
            return (targets, consensus);
    }
        
    private static ForecastTarget Map(GetForecastResponse.Types.TargetItem targetItem) =>
        new()
        {
            InstrumentId = Guid.Parse(targetItem.Uid),
            Ticker = targetItem.Ticker,
            Company = targetItem.Company,
            RecommendationDate = ConvertHelper.TimestampToDateOnly(targetItem.RecommendationDate),
            Currency = targetItem.Currency,
            CurrentPrice = ConvertHelper.QuotationToDouble(targetItem.CurrentPrice),
            TargetPrice = ConvertHelper.QuotationToDouble(targetItem.TargetPrice),
            PriceChange = ConvertHelper.QuotationToDouble(targetItem.PriceChange),
            PriceChangeRel = ConvertHelper.QuotationToDouble(targetItem.PriceChangeRel),
            ShowName = targetItem.ShowName,
            RecommendationNumber = targetItem.Recommendation switch
            {
                Recommendation.Buy => 1,
                Recommendation.Hold => 2,
                Recommendation.Sell => 3,
                _ => 0
            },
            RecommendationString = targetItem.Recommendation switch
            {
                Recommendation.Buy => KnownForecastRecommendations.Buy,
                Recommendation.Hold => KnownForecastRecommendations.Hold,
                Recommendation.Sell => KnownForecastRecommendations.Sell,
                _ => string.Empty
            }
        };
    
    private static ForecastConsensus Map(GetForecastResponse.Types.ConsensusItem consensusItem) =>
        new()
        {
            InstrumentId = Guid.Parse(consensusItem.Uid),
            Ticker = consensusItem.Ticker,
            Currency = consensusItem.Currency,
            CurrentPrice = ConvertHelper.QuotationToDouble(consensusItem.CurrentPrice),
            MinTarget = ConvertHelper.QuotationToDouble(consensusItem.MinTarget),
            MaxTarget = ConvertHelper.QuotationToDouble(consensusItem.MaxTarget),
            PriceChange = ConvertHelper.QuotationToDouble(consensusItem.PriceChange),
            PriceChangeRel = ConvertHelper.QuotationToDouble(consensusItem.PriceChangeRel),
            RecommendationNumber = consensusItem.Recommendation switch
            {
                Recommendation.Buy => 1,
                Recommendation.Hold => 2,
                Recommendation.Sell => 3,
                _ => 0
            },
            RecommendationString = consensusItem.Recommendation switch
            {
                Recommendation.Buy => KnownForecastRecommendations.Buy,
                Recommendation.Hold => KnownForecastRecommendations.Hold,
                Recommendation.Sell => KnownForecastRecommendations.Sell,
                _ => string.Empty
            }
        };
    
    private static GetForecastRequest CreateGetForecastRequest(Guid instrumentId) =>
        new()
        {
            InstrumentId = instrumentId.ToString()
        };
    
    private async Task<GetForecastResponse?> SendGetForecastRequest(GetForecastRequest request)
    {
        try
        {
            return await client.Instruments.GetForecastByAsync(request);
        }
        
        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка получения данных. {request}", request);
            return null;
        }
    }
}