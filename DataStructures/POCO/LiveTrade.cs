using System;

namespace DataStructures.POCO
{
    public class LiveTrade
    {
        #region Properties

        public string Symbol { get; set; }
        public double Position { get; set; }
        public float MarketPrice { get; set; }
        public float MarketValue { get; set; }
        public float AverageCost { get; set; }
        public float UnrealizedPnL { get; set; }
        public float RealizedPnl { get; set; }
        public string Account { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Port { get; set; }

        #endregion
    }
}