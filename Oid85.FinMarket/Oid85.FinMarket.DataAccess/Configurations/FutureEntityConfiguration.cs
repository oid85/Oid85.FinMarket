﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class FutureEntityConfiguration : EntityConfigurationBase<FutureEntity>
{
    public override void Configure(EntityTypeBuilder<FutureEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("futures", KnownDatabaseSchemas.Default);
    }
}