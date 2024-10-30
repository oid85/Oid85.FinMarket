using System.ComponentModel.DataAnnotations.Schema;

namespace Oid85.FinMarket.DataAccess.Entities.Base;

public class AuditableEntity : BaseEntity
{
    [Column("created_at", TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Column("updated_at", TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    [Column("deleted_at", TypeName = "timestamp with time zone")]
    public DateTime DeletedAt { get; set; } = DateTime.MinValue.ToUniversalTime();    
    
    [Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;
}