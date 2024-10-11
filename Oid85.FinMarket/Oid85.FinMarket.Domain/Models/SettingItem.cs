namespace Oid85.FinMarket.Domain.Models
{
    public class SettingItem
    {
        public long Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
