using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class FinIndexEntityConfiguration : EntityConfigurationBase<FinIndexEntity>
{
    private readonly FinIndexEntity[] _customIndexes =
    [
        new()
        {
            Id = Guid.Parse("9125d7a7-cdd1-4e5d-949f-c34cfb09b984"),
            InstrumentId = KnownInstrumentIds.OilAndGasSectorIndex,
            Name = "Oil and Gas Sector Index",  
            Ticker = "OGSI"
        },
        
        new()
        {
            Id = Guid.Parse("a70dd4da-cb55-47f2-ac00-fefd08bc30db"),
            InstrumentId = KnownInstrumentIds.BanksSectorIndex,
            Name = "Banks Sector Index",  
            Ticker = "BSI"
        },
        
        new()
        {
            Id = Guid.Parse("ed81ffb7-166c-4109-aa1f-11ff2021d214"),
            InstrumentId = KnownInstrumentIds.EnergSectorIndex,
            Name = "Energ Sector Index",  
            Ticker = "ESI"
        },
        
        new()
        {
            Id = Guid.Parse("923ada63-22f4-429f-b50f-3ac61b2cb457"),
            InstrumentId = KnownInstrumentIds.FinanceSectorIndex,
            Name = "Finance Sector Index",  
            Ticker = "FSI"
        },
        
        new()
        {
            Id = Guid.Parse("2eb79a47-add7-40cb-8db6-3ffb43d4bee3"),
            InstrumentId = KnownInstrumentIds.HousingAndUtilitiesSectorIndex,
            Name = "Housing And Utilities Sector Index",  
            Ticker = "HUSI"
        },
        
        new()
        {
            Id = Guid.Parse("7002e1b6-3709-4b79-9598-0b87d50bdd52"),
            InstrumentId = KnownInstrumentIds.IronAndSteelIndustrySectorIndex,
            Name = "IronAndSteelIndustry Sector Index",  
            Ticker = "ISISI"
        },
        
        new()
        {
            Id = Guid.Parse("521aacd7-aa21-4b10-903b-ba2cfd97e2f1"),
            InstrumentId = KnownInstrumentIds.ItSectorIndex,
            Name = "IT Sector Index",  
            Ticker = "ITSI"
        },
        
        new()
        {
            Id = Guid.Parse("20d22fe7-8870-4579-af1e-6dc4e6c06f9e"),
            InstrumentId = KnownInstrumentIds.MiningSectorIndex,
            Name = "Mining Sector Index",  
            Ticker = "MSI"
        },
        
        new()
        {
            Id = Guid.Parse("9a76fcdf-7e10-48c0-aa08-2b8a4bf2e519"),
            InstrumentId = KnownInstrumentIds.NonFerrousMetallurgySectorIndex,
            Name = "Non Ferrous Metallurgy Sector Index",  
            Ticker = "NFMSI"
        },
        
        new()
        {
            Id = Guid.Parse("8e237dbb-d8df-4599-ad6e-4635cb891ef5"),
            InstrumentId = KnownInstrumentIds.RetailSectorIndex,
            Name = "Retail Sector Index",  
            Ticker = "RSI"
        },
        
        new()
        {
            Id = Guid.Parse("2b063b20-da30-407b-8a5a-1de917d6e889"),
            InstrumentId = KnownInstrumentIds.TelecomSectorIndex,
            Name = "Telecom Sector Index",  
            Ticker = "TSI"
        },
        
        new()
        {
            Id = Guid.Parse("4c968e87-2c9e-43b2-a896-09798c74b082"),
            InstrumentId = KnownInstrumentIds.TransportSectorIndex,
            Name = "Transport Sector Index",  
            Ticker = "TRSI"
        }
    ];
        
    public override void Configure(EntityTypeBuilder<FinIndexEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("fin_indexes", KnownDatabaseSchemas.Default);
        
        builder.HasData(_customIndexes);
    }
}