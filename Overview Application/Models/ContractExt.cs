using System;
using IBApi;

namespace OverviewApp.Models
{
    public class ContractExt : Contract
    {
        #region Properties

        public DateTime RolloverDate { get; set; }
        public string TickerId { get; set; }
        public double DblMultiplier { get; set; }

        #endregion Properties
    }
}