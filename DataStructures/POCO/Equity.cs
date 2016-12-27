using System;

namespace DataStructures.POCO
{
    public class Equity
    {
        #region Properties

        public int Id { get; set; }
        public string Account { get; set; }
        public double Value { get; set; }
        public DateTime UpdateTime { get; set; }

        #endregion
    }
}