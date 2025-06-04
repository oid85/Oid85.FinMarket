namespace Oid85.FinMarket.WealthLab.Centaur.Indicators.Types
{
    /// <summary>
    /// Свеча
    /// </summary>
    public struct Candle
    {
        /// <summary>
        /// Цена открытия свечи
        /// </summary>
        public double Open;

        /// <summary>
        /// Цена закрытия свечи
        /// </summary>
        public double Close;

        /// <summary>
        /// Максимум цены свечи
        /// </summary>
        public double High;

        /// <summary>
        /// Минимум цены свечи
        /// </summary>
        public double Low;

        /// <summary>
        /// Объем на свече
        /// </summary>
        public double Volume;

        /// <summary>
        /// Дата свечи
        /// </summary>
        public DateTime Date;
    }
}
