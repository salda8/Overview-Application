using System;

namespace DataStructures.POCO
{
    public class Candlestick
    {
        #region Properties

        public int Id { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public DateTime BarTime { get; set; }
        public string Symbol { get; set; }
        public DateTime DbTime { get; set; }
        public int Interval { get; set; }
        public long Volume { get; set; }

        #endregion
    }
}