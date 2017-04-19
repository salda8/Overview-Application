﻿using System.ComponentModel;
using Common.EntityModels;
using Common.Interfaces;

namespace OverviewApp.Model
{
    public class AccountPl : IEntity
    {
        public int ID { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        public string BrokerName { get; set; }

        public decimal InitialBalance { get; set; }

        public virtual Strategy Strategy { get; set; }

        public int StrategyID { get; set; }
    }
}