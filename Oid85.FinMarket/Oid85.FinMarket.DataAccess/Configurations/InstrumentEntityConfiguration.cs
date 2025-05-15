using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class InstrumentEntityConfiguration : EntityConfigurationBase<InstrumentEntity>
{
    private readonly InstrumentEntity[] _customInstruments = {
        new()
        {
            Id = Guid.Parse("05804fbf-35a1-481c-bbf2-4acfc3996da3"),
            InstrumentId = KnownInstrumentIds.OilAndGasSectorIndex,
            Name = "Oil and Gas Sector Index",  
            Ticker = "OGSI",
            Type = "Index"
        },
        
        new()
        {
            Id = Guid.Parse("3a20bcb8-6be5-4510-b91a-f3096918686c"),
            InstrumentId = KnownInstrumentIds.BanksSectorIndex,
            Name = "Banks Sector Index",  
            Ticker = "BSI",
            Type = "Index"
        },
        
        new()
        {
            Id = Guid.Parse("abe702e9-0271-4622-9f52-4de2da88ebfc"),
            InstrumentId = KnownInstrumentIds.EnergSectorIndex,
            Name = "Energ Sector Index",  
            Ticker = "ESI",
            Type = "Index"
        },
        
        new()
        {
            Id = Guid.Parse("271bb1d1-4476-4638-bae8-0cac6f7179ad"),
            InstrumentId = KnownInstrumentIds.FinanceSectorIndex,
            Name = "Finance Sector Index",  
            Ticker = "FSI",
            Type = "Index"
        },
        
        new()
        {
            Id = Guid.Parse("d05f0a65-9d0c-42dc-8dd1-130ba5bfeb3e"),
            InstrumentId = KnownInstrumentIds.HousingAndUtilitiesSectorIndex,
            Name = "Housing And Utilities Sector Index",  
            Ticker = "HUSI",
            Type = "Index"
        },
        
        new()
        {
            Id = Guid.Parse("d2615eb5-c224-4e7d-9bb7-8bfa4e1351ea"),
            InstrumentId = KnownInstrumentIds.IronAndSteelIndustrySectorIndex,
            Name = "IronAndSteelIndustry Sector Index",  
            Ticker = "ISISI",
            Type = "Index"
        },
        
        new()
        {
            Id = Guid.Parse("712e7169-5953-46a0-829e-400b9015a56d"),
            InstrumentId = KnownInstrumentIds.ItSectorIndex,
            Name = "IT Sector Index",  
            Ticker = "ITSI",
            Type = "Index"
        },
        
        new()
        {
            Id = Guid.Parse("c3db74d9-048b-4fe7-9abe-3d67a1b4010f"),
            InstrumentId = KnownInstrumentIds.MiningSectorIndex,
            Name = "Mining Sector Index",  
            Ticker = "MSI",
            Type = "Index"
        },
        
        new()
        {
            Id = Guid.Parse("97094d1f-8426-44fc-a25d-a556ec9c3a97"),
            InstrumentId = KnownInstrumentIds.NonFerrousMetallurgySectorIndex,
            Name = "Non Ferrous Metallurgy Sector Index",  
            Ticker = "NFMSI",
            Type = "Index"
        },
        
        new()
        {
            Id = Guid.Parse("e0683c7c-68b0-4d9a-a3b4-8f94086df49f"),
            InstrumentId = KnownInstrumentIds.RetailSectorIndex,
            Name = "Retail Sector Index",  
            Ticker = "RSI",
            Type = "Index"
        },
        
        new()
        {
            Id = Guid.Parse("d9edc8e2-df33-484d-b509-74b55c44396d"),
            InstrumentId = KnownInstrumentIds.TelecomSectorIndex,
            Name = "Telecom Sector Index",  
            Ticker = "TSI",
            Type = "Index"
        }
    };

    public override void Configure(EntityTypeBuilder<InstrumentEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("instruments", KnownDatabaseSchemas.Default);

        builder.HasData(_customInstruments);
    }
}