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
        public int Id { get; set; }

        /// <summary>
        /// Тикер
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// Идентификатор FIGI
        /// </summary>
        public string Figi { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Флаг активности (1 - активен, 0 - не активен)
        /// </summary>
        public int IsActive { get; set; }
    }
}
