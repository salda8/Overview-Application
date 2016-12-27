using System;

namespace DataStructures.POCO
{
    public class OpenOrder
    {
        #region Properties

        public int PermId { get; set; }
        public string Symbol { get; set; }
        public string Status { get; set; }
        public float LimitPrice { get; set; }
        public double Position { get; set; }
        public string Account { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }

        #endregion
    }
}