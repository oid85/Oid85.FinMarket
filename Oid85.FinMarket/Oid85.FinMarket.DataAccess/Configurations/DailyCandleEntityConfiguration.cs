﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class DailyCandleEntityConfiguration : EntityConfigurationBase<DailyCandleEntity>
{
    public override void Configure(EntityTypeBuilder<DailyCandleEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("daily_candles", KnownDatabaseSchemas.Default);
        builder.HasIndex(x => x.InstrumentId);
    }
}