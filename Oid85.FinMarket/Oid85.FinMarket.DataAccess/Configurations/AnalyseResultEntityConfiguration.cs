﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class AnalyseResultEntityConfiguration : EntityConfigurationBase<AnalyseResultEntity>
{
    public override void Configure(EntityTypeBuilder<AnalyseResultEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("analyse_results", KnownDatabaseSchemas.Storage);
        builder.HasIndex(x => x.Ticker);
    }
}