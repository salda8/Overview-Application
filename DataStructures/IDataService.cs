using QDMS;
using System.Collections.ObjectModel;

namespace DataStructures
{
    public interface IDataService : IDataConnection
    {
        ObservableCollection<PortfolioSummary> GetPortfolioSummary();

        ObservableCollection<PortfolioSummary> GetPortfolioSummary(string acc);

        ObservableCollection<TradeHistory> GetTradeHistory(int id = 0);

        ObservableCollection<TradeHistory> GetTradeHistory(string acc);

        ObservableCollection<LiveTrade> GetLiveTrades();

        ObservableCollection<LiveTrade> GetLiveTrades(string acc);

        ObservableCollection<OpenOrder> GetOpenOrders();

        ObservableCollection<OpenOrder> GetOpenOrders(string acc);

        ObservableCollection<Equity> GetEquity(int id = 0);

        ObservableCollection<Candlestick> GetBars(int id = 0);

        ObservableCollection<Matlabvalue> GetMatlabvalues();

        ObservableCollection<Strategy> GetStrategy();

        void ReqGlobalCancel(string acc);
    }

    public interface IDataConnection
    {
    }
}