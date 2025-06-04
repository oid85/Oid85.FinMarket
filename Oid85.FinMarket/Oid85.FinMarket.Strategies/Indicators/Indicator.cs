using Oid85.FinMarket.Strategies.Models;
using WealthLab;

namespace Oid85.FinMarket.Strategies.Indicators
{
    public class Indicator : IIndicator
    {
        public List<double> Values { get; set; }

        /// <summary>
        /// Инициализировать бары
        /// </summary>
        public Bars Init(List<Candle> candles)
        {
            var bars = new Bars("bars", BarScale.Minute, 1);

            foreach (Candle candle in candles)
                bars.Add(candle.DateTime, candle.Open, candle.High, candle.Low, candle.Close, candle.Volume);
            
            return bars;
        }

        /// <summary>
        /// Инициализировать серию
        /// </summary>
        public DataSeries Init(List<double> source)
        {
            var ds = new DataSeries("ds");

            for (int i = 0; i < source.Count; i++)
                ds.Add(source[i]);

            return ds;
        }

        /// <summary>
        /// Преобразовать индикатор из формата WealthLab
        /// </summary>
        public List<double> ToIndicator(DataSeries dataSeries)
        {
            List<double> indicator = new List<double>();
            
            for (int bar = 0; bar < dataSeries.Count; bar++)
                indicator.Add(dataSeries[bar]);
            
            return indicator;
        }
    }
}

