using AutoMapper;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Mapping;

public class FinMarketMappingProfile : Profile
{
    public FinMarketMappingProfile()
    {
        CreateMap<AnalyseResult, AnalyseResultEntity>();
        CreateMap<Bond, BondEntity>();
        CreateMap<Candle, CandleEntity>();
        CreateMap<DividendInfo, DividendInfoEntity>();
        CreateMap<Share, ShareEntity>();
        CreateMap<Timeframe, TimeframeEntity>();
    }
}