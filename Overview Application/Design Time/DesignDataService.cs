using Common;
using OverviewApp.TradingEntitiesPl;
using System;
using System.Collections.ObjectModel;

namespace OverviewApp.Design_Time
{
    public class DesignContext
    {
        #region

        public ObservableCollection<PortfolioSummary> GetPortfolioSummary()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<PortfolioSummary> GetPortfolioSummary(string acc)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<TradeHistory> GetTradeHistory(int id = 0)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<TradeHistory> GetTradeHistory(string acc)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<LiveTrade> GetLiveTrades()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<LiveTrade> GetLiveTrades(string acc)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<OpenOrder> GetOpenOrders()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<OpenOrder> GetOpenOrders(string acc)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Equity> GetEquity(int id = 0)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Candlestick> GetBars(int id = 0)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<MatlabvaluePl> GetMatlabvalues()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Strategy> GetStrategy()
        {
            throw new NotImplementedException();
        }

        public void ReqGlobalCancel(string acc)
        {
            throw new NotImplementedException();
        }

        #endregion

        //public void GetData(Action<DataItem, Exception> callback)
        //{
        //    // Use this to create design time data

        //    var item = new DataItem("Welcome to MVVM Light [design]");
        //    callback(item, null);
        //}

        public ObservableCollection<string> GetNames(int anAmount)
        {
            var col = new ObservableCollection<string>();
            for (var i = 0; i < 5; i++)
            {
                col.Add("Design name: " + (i + 1));
            }
            return col;
        }

        public ObservableCollection<TradeHistory> GetTradeHistory()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Equity> GetEquity()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Candlestick> GetBars()
        {
            throw new NotImplementedException();
        }
    }
}