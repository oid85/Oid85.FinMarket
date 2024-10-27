using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class TimeframeEntityConfiguration : EntityConfigurationBase<TimeframeEntity>
{
    public override void Configure(EntityTypeBuilder<TimeframeEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("timeframes", KnownDatabaseSchemas.Dictionaries);

        builder
            .HasData(
                new TimeframeEntity
                {
                    Id = Guid.Parse("eaa80987-548b-474d-8882-9003a10db167"),
                    Name = "D",
                    Description = "1 день"
                },
                new TimeframeEntity
                {
                    Id = Guid.Parse("827adf38-2f99-4066-ba5c-33a646d2767b"),
                    Name = "H",
                    Description = "1 час"
                });
    }
}