using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Domain.Mapping;

public static class DomainMapper
{
    public static Instrument Map(Share model) =>
        new()
        {
            InstrumentId = model.InstrumentId,
            Ticker = model.Ticker,
            Name = model.Name,
            Sector = model.Sector,
            Type = KnownInstrumentTypes.Share
        };
    
    public static Instrument Map(Bond model) =>
        new()
        {
            InstrumentId = model.InstrumentId,
            Ticker = model.Ticker,
            Name = model.Name,
            Sector = model.Sector,
            Type = KnownInstrumentTypes.Bond
        };    
    
    public static Instrument Map(Future model) =>
        new()
        {
            InstrumentId = model.InstrumentId,
            Ticker = model.Ticker,
            Name = model.Name,
            Sector = string.Empty,
            Type = KnownInstrumentTypes.Future
        };    
    
    public static Instrument Map(Currency model) =>
        new()
        {
            InstrumentId = model.InstrumentId,
            Ticker = model.Ticker,
            Name = model.Name,
            Sector = string.Empty,
            Type = KnownInstrumentTypes.Currency
        };  
    
    public static Instrument Map(FinIndex model) =>
        new()
        {
            InstrumentId = model.InstrumentId,
            Ticker = model.Ticker,
            Name = model.Name,
            Sector = string.Empty,
            Type = KnownInstrumentTypes.Index
        };
}