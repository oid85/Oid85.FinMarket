using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class InstrumentEntityConfiguration : EntityConfigurationBase<InstrumentEntity>
{
    private InstrumentEntity[] _customInstruments = new []
    {
        new InstrumentEntity()
        {
            Id = Guid.Parse("05804fbf-35a1-481c-bbf2-4acfc3996da3"),
            InstrumentId = KnownInstrumentIds.OilAndGasSectorIndex,
            Name = "Oil and Gas Sector Index",  
            Ticker = "OGSI",
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