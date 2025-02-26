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
        try
        {
            await Task.Delay(DelayInMilliseconds);
            
            var request = new GetForecastRequest
            {
                InstrumentId = instrumentId.ToString()
            };
            
            var response = await client.Instruments.GetForecastByAsync(request);

            if (response is null)
                return ([], new());

            var targets = new List<ForecastTarget>();
            
            foreach (var targetItem in response.Targets)
            {
                var target = new ForecastTarget
                {
                    InstrumentId = Guid.Parse(targetItem.Uid),
                    Ticker = targetItem.Ticker,
                    Company = targetItem.Company
                };

                switch (targetItem.Recommendation)
                {
                    case Recommendation.Unspecified:
                        target.RecommendationString = KnownForecastRecommendations.Unknown;
                        target.RecommendationNumber = 0;
                        break;
                    
                    case Recommendation.Buy:
                        target.RecommendationString = KnownForecastRecommendations.Buy;
                        target.RecommendationNumber = 1;
                        break;
                    
                    case Recommendation.Hold:
                        target.RecommendationString = KnownForecastRecommendations.Hold;
                        target.RecommendationNumber = 2;
                        break;
                    
                    case Recommendation.Sell:
                        target.RecommendationString = KnownForecastRecommendations.Sell;
                        target.RecommendationNumber = 3;
                        break;
                }
                
                target.RecommendationDate = ConvertHelper.TimestampToDateOnly(targetItem.RecommendationDate);
                target.Currency = targetItem.Currency;
                target.CurrentPrice = ConvertHelper.QuotationToDouble(targetItem.CurrentPrice);
                target.TargetPrice = ConvertHelper.QuotationToDouble(targetItem.TargetPrice);
                target.PriceChange = ConvertHelper.QuotationToDouble(targetItem.PriceChange);
                target.PriceChangeRel = ConvertHelper.QuotationToDouble(targetItem.PriceChangeRel);
                target.ShowName = targetItem.ShowName;
                
                targets.Add(target);
            }
            
            var consensus = new ForecastConsensus
            {
                InstrumentId = Guid.Parse(response.Consensus.Uid),
                Ticker = response.Consensus.Ticker
            };

            switch (response.Consensus.Recommendation)
            {
                case Recommendation.Unspecified:
                    consensus.RecommendationString = KnownForecastRecommendations.Unknown;
                    consensus.RecommendationNumber = 0;
                    break;
                    
                case Recommendation.Buy:
                    consensus.RecommendationString = KnownForecastRecommendations.Buy;
                    consensus.RecommendationNumber = 1;
                    break;
                    
                case Recommendation.Hold:
                    consensus.RecommendationString = KnownForecastRecommendations.Hold;
                    consensus.RecommendationNumber = 2;
                    break;
                    
                case Recommendation.Sell:
                    consensus.RecommendationString = KnownForecastRecommendations.Sell;
                    consensus.RecommendationNumber = 3;
                    break;
            }
            
            consensus.Currency = response.Consensus.Currency;
            consensus.CurrentPrice = ConvertHelper.QuotationToDouble(response.Consensus.CurrentPrice);
            consensus.ConsensusPrice = ConvertHelper.QuotationToDouble(response.Consensus.Consensus);
            consensus.MinTarget = ConvertHelper.QuotationToDouble(response.Consensus.MinTarget);
            consensus.MaxTarget = ConvertHelper.QuotationToDouble(response.Consensus.MaxTarget);
            consensus.PriceChange = ConvertHelper.QuotationToDouble(response.Consensus.PriceChange);
            consensus.PriceChangeRel = ConvertHelper.QuotationToDouble(response.Consensus.PriceChangeRel);
            
            return (targets, consensus);
        }
        
        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка получения данных. {instrumentId}", instrumentId);
            return ([], new());
        }
    }
}