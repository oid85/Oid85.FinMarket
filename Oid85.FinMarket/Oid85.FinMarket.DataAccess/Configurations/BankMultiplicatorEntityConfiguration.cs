using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class BankMultiplicatorEntityConfiguration : EntityConfigurationBase<BankMultiplicatorEntity>
{
    public override void Configure(EntityTypeBuilder<BankMultiplicatorEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("bank_multiplicators", KnownDatabaseSchemas.Default);
    }
}