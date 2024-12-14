using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class BondCouponEntityConfiguration : EntityConfigurationBase<BondCouponEntity>
{
    public override void Configure(EntityTypeBuilder<BondCouponEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("bond_coupons", KnownDatabaseSchemas.Default);
    }
}