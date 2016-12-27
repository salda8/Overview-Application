using System;

namespace DataStructures.POCO
{
    public class TradeHistory
    {
        #region Properties

        public string ExecId { get; set; }
        public double Position { get; set; }
        public string Side { get; set; }
        public string Symbol { get; set; }
        public float Price { get; set; }
        public double Commission { get; set; }
        public float RealizedPnL { get; set; }
        public DateTime ExecTime { get; set; }
        public string Account { get; set; }
        public DateTime DbTime { get; set; }
        public int Id { get; set; }

        #endregion
    }
}