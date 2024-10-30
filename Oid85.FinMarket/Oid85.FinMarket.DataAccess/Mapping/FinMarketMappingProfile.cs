using AutoMapper;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Mapping;

public class FinMarketMappingProfile : Profile
{
    public FinMarketMappingProfile()
    {
        CreateMap<AnalyseResultEntity, AnalyseResult>()
            .ReverseMap();
        
        CreateMap<BondEntity, Bond>()
            .ReverseMap();
        
        CreateMap<CandleEntity, Candle>()
            .ReverseMap();
        
        CreateMap<DividendInfoEntity, DividendInfo>()
            .ReverseMap();
        
        CreateMap<ShareEntity, Share>()
            .ReverseMap();
    }
}