namespace Oid85.FinMarket.Domain.Models
{
    /// <summary>
    /// Финансовый инструмент
    /// </summary>
    public class FinancicalInstrument
    {
        /// <summary>
        /// Идентификатор в справочнике
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Тикер
        /// </summary>
        public string Ticker { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор FIGI
        /// </summary>
        public string Figi { get; set; } = string.Empty;

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Сектор
        /// </summary>
        public string Sector { get; set; } = string.Empty;

        /// <summary>
        /// Флаг активности (1 - активен, 0 - не активен)
        /// </summary>
        public int IsActive { get; set; }
    }
}
