using System;
using System.ComponentModel;

namespace OverviewApp.Model
{
    public class EquityPl
    {
        public int ID { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        public int AccountID { get; set; }

        [DisplayName("Value")]
        public decimal Value { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}