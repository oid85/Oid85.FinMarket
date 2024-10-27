﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class DividendInfoEntityConfiguration : EntityConfigurationBase<ShareEntity>
{
    public override void Configure(EntityTypeBuilder<ShareEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("dividend_info", KnownDatabaseSchemas.Default);
        
        builder
            .HasMany(x => x.DividendInfoEntities)
            .WithOne(x => x.Share)
            .HasForeignKey(x => x.ShareId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}