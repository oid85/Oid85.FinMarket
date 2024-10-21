namespace Oid85.FinMarket.Domain.Models
{
    /// <summary>
    /// Информация по дивидендам
    /// </summary>
    public class DividendInfo
    {
        /// <summary>
        /// Тикер
        /// </summary>
        public string Ticker { get; set; } = string.Empty;

        /// <summary>
        /// Дата объявления дивидендов
        /// </summary>
        public DateTime DividendDate { get; set; }

        /// <summary>
        /// Выплата, руб
        /// </summary>
        public double Dividend { get; set; }

        /// <summary>
        /// Доходность, %
        /// </summary>
        public double DividendPrc { get; set; }
    }
}
